export class RegisterModel{
    email : string;
    password: string;
    name : string

    constructor(_name: string, _email: string, _password: string){
        this.name = _name;
        this.email = _email;
        this.password = _password;
    }
}