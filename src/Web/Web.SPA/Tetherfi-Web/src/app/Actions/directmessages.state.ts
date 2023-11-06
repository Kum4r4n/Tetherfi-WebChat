import { ChatUserModel } from "../Models/ChatUserModel";
import { DirectMessage } from "../Models/DirectMessage";

export interface DirectMessagesState {
  onlineUsers: ChatUserModel[];
  directMessages: DirectMessage[];
  connected: boolean;
}