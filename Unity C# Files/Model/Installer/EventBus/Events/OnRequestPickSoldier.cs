using System.Collections.Generic;

namespace GameLogic.Scripts.EventBus.Events
{
    public record OnRequestPickSoldier
    {
        public int choicesAmount;
        public bool keepsAll;
        public string instructions;
        public string buttonText;
        public OnRequestPickSoldier(int choicesAmount, bool giveAll, string instructions, string buttonText)
        {
            this.choicesAmount = choicesAmount;
            this.keepsAll = giveAll;
            this.instructions = instructions;
            this.buttonText = buttonText;
        }
    }

}