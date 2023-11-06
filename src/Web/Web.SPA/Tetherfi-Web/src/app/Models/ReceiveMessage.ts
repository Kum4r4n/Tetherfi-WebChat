import { ChatUserModel } from "./ChatUserModel";

export class ReceiveMessage {

    user : ChatUserModel = {connectionId: "", id : "",name : "",};
    message : string = "";
}