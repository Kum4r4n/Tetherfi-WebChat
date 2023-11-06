import { ChatUserModel } from "./ChatUserModel";

export class DirectMessage {
    public fromOnlineUser: ChatUserModel | null = { connectionId: '', id: '', name: ''};
    public message = '';
  }