import { AfterViewInit, Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SignalingService } from '../Services/Signaling.service';
import { Message } from '../Models/Message';


const mediaConstaints = {
  audio : true,
  video : {
    width:720,
    height:540
  }
};

const offerOptions = {
  offerToReceiveAudio: true,
  offerToReceiveVideo: false
};

@Component({
  selector: 'app-call-popup',
  templateUrl: './call-popup.component.html',
  styleUrls: ['./call-popup.component.css']
})
export class CallPopupComponent  implements AfterViewInit{

  private localStream : MediaStream | undefined;
  @ViewChild('local_video') localVideo : ElementRef | null = null;
  @ViewChild('received_video') remoteVideo : ElementRef | null = null;

  private peerConnection : RTCPeerConnection | undefined; 

  constructor(
    public dialogRef: MatDialogRef<CallPopupComponent>, @Inject(MAT_DIALOG_DATA) public partnerId: any,  private signalingService : SignalingService
   ) {

      this.dialogRef.disableClose = true;
      this.dialogRef.afterClosed().subscribe(data=>{

        console.log("closed");
      });
    }

  ngAfterViewInit(): void {
    this.addIncommingMessageHandler();
    this.requestMediaDevices();
    
  }

  async requestMediaDevices() : Promise<void>{
    this.localStream = await navigator.mediaDevices.getUserMedia(mediaConstaints);
    this.pauseLocalVideo();
    //this.startLocalVideo();
    //this.call();
  }

  async hangUp(): Promise<void> {
    this.signalingService.sendMessage({type : "hangup", data:''},this.partnerId);
    this.pauseLocalVideo();
    this.closeVideoCall();
    this.dialogRef.close();
  }

  startLocalVideo() : void{
    this.localStream!.getTracks().forEach(track => {
      track.enabled = true;
    });

    this.localVideo!.nativeElement.srcObject = this.localStream;
  }

  pauseLocalVideo() : void{
    this.localStream!.getTracks().forEach(track => {
      track.enabled = false;
    });

    this.localVideo!.nativeElement.srcObject = null;
  }

  closeVideoCall() : void{
    if(this.peerConnection){
      this.peerConnection.onicecandidate = null;
      this.peerConnection.oniceconnectionstatechange = null;
      this.peerConnection.onsignalingstatechange = null;
      this.peerConnection.ontrack = null;

      this.peerConnection?.getTransceivers().forEach(transcevier => {
        transcevier.stop();
      });
  
      this.peerConnection?.close();
      this.peerConnection = undefined;
    }
    
  }

  createPeerConnection(){

    this.peerConnection = new RTCPeerConnection({
      iceServers :[{
        urls :['stun:stun1.l.google.com:19302']
      }]
    });

    this.peerConnection.onicecandidate = this.handleICECandidateEvent;
    this.peerConnection.oniceconnectionstatechange = this.handleICEConnectionStateChangeEvent;
    this.peerConnection.onsignalingstatechange = this.handleSignalingStateEvent;
    this.peerConnection.ontrack = this.handleTrackEvent;

  }

  handleICECandidateEvent = (event:RTCPeerConnectionIceEvent)=>{
    console.log(event);
    if(event.candidate){
      this.signalingService.sendMessage({type : "ice-candidate", data : event.candidate}, this.partnerId);
    }
  }

  handleICEConnectionStateChangeEvent = (event : Event)=>{
    console.log(event);
    switch(this.peerConnection?.iceConnectionState){
      case "failed" :
      case "disconnected" :
      case "closed" :
      this.closeVideoCall();
      break;
    }
  }

  handleSignalingStateEvent = (event : Event)=>{
    console.log(event);
    switch(this.peerConnection!.signalingState){
      case "closed" :
        this.closeVideoCall();
        break;
    }
  }

  handleTrackEvent = (event : RTCTrackEvent)=>{
    console.log(event);
    this.remoteVideo!.nativeElement.srcObject = event.streams[0];
  }

  async call() : Promise<void>{
    this.createPeerConnection();
    this.localStream!.getTracks().forEach(track => this.peerConnection!.addTrack(track, this.localStream!));

    try{

      const offer : RTCSessionDescriptionInit  = await this.peerConnection!.createOffer(offerOptions);
      await this.peerConnection!.setLocalDescription(offer);

      this.signalingService.sendMessage({type : "offer", data : offer}, this.partnerId);

    }catch(error : any){
      this.handleGetUserMediaError(error);
    }
    
  }

  handleGetUserMediaError(error : Error) : void{
    switch (error.name) {
      case ' NotFoundError' :
      alert('unable to open your call because no camera and/or microphone were found.');
      break;
      case 'SecurityError' :
      case 'PermissionDeniedError' :
      // Do nothing
      break;
      default:
      console.log(error) ;
      alert( 'Error opening your camera' + error.message);
      break;
    }

    this.closeVideoCall();
  }

  addIncommingMessageHandler() : void{
    this.signalingService.init();
    this.signalingService.messages$.subscribe(message => {
      switch(message.type){
        case "offer" :
        this.handleOffer(message.data);
        break;
        case "answer" :
        this.handleAnswer(message.data);
        break;
        case "hangup" :
        this.handleHangUp(message);
        break;
        case "ice-candidate" :
        this.handleICECandidate(message.data);
        break;
        default:
          console.log('unknown message type ' + message.type);
      }
    });
  }

  handleOffer(msg : RTCSessionDescriptionInit) : void{
    if(!this.peerConnection){
      this.createPeerConnection();
    }

    if(!this.localStream){
      this.startLocalVideo();
    }

    this.peerConnection!.setRemoteDescription(new RTCSessionDescription(msg))
    .then(() => {

      // add media stream to local video
      this.localVideo!.nativeElement.srcObject = this.localStream;

      // add media tracks to remote connection
      this.localStream!.getTracks().forEach(
        track => this.peerConnection!.addTrack(track, this.localStream!)
      );

    }).then(() => {

    // Build SDP for answer message
    return this.peerConnection!.createAnswer();

  }).then((answer) => {

    // Set local SDP
    return this.peerConnection!.setLocalDescription(answer);

  }).then(() => {

    this.peerConnection?.setRemoteDescription
    // Send local SDP to remote party
    this.signalingService.sendMessage({type: 'answer', data: this.peerConnection!.localDescription},this.partnerId);


  }).catch(this.handleGetUserMediaError);
  }

  handleAnswer(data : RTCSessionDescriptionInit) : void{
    this.peerConnection!.setRemoteDescription(data);
    this.remoteVideo!.nativeElement.srcObject = data;
  }

  handleHangUp(data : Message) : void{
    this.closeVideoCall();
  }

  handleICECandidate(data : RTCIceCandidate) : void{
    const candidate = new RTCIceCandidate(data);
    this.peerConnection!.addIceCandidate(candidate).catch(this.reportError);
  }

  reportError(error : any) : void{
    console.log("got error : "+error.name);
    console.log(error);
  }
}
