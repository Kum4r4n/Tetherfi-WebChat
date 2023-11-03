﻿namespace Identity.Application.Interfaces.Services
{
    public interface IPasswordService
    {
        string Hash(string password);
        bool Verify(string password, string hash);

    }
}
