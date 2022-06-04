﻿using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.DAL.Entities;
using System;
using System.Linq;

namespace MinecraftServersControl.Core
{
    internal static class DTOExtensions
    {
        public static SessionDTO ToSessionDTO(this Session self)
        {
            return new SessionDTO(new Guid(self.Id), self.ExpiresAt.ToDateTime());
        }
    }
}
