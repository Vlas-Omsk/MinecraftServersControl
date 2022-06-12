﻿using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class ComputerStateDTO
    {
        public Guid ComputerId { get; private set; }

        private ComputerStateDTO()
        {
        }

        public ComputerStateDTO(Guid computerId)
        {
            ComputerId = computerId;
        }
    }
}
