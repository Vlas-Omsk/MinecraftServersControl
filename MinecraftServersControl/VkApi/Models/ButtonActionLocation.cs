using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ButtonActionLocation : ButtonAction
    {
        public override ButtonActionType Type => ButtonActionType.Location;
    }
}
