﻿namespace Blablatec.Infra.Authorize
{
    public interface IUser
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}