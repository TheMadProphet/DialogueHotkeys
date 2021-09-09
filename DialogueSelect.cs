using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.InputSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;


namespace DialogueHotkeys
{
    class DialogueSelect : MBSubModuleBase
    {
        private ConversationManager conversationManager = null;

        private readonly bool debug = false;
        private readonly InputKey debugKey = InputKey.D0;

        protected override void OnApplicationTick(float dt)
        {
            if (conversationManager != null)
            {
                if (conversationManager.IsConversationInProgress)
                {
                    if (conversationManager.CurOptions.Count == 0)
                    {
                        if (Input.IsKeyPressed(InputKey.Space))
                        {
                            conversationManager.ContinueConversation();
                            Debug("Continue conversation");
                        }
                    }
                    else
                    {
                        for (InputKey inpKey = InputKey.D1; inpKey <= InputKey.D9 && (int)inpKey - (int)InputKey.D1 < conversationManager.CurOptions.Count; inpKey++)
                        {
                            if (Input.IsKeyPressed(inpKey))
                            {
                                int index = (int)inpKey - (int)InputKey.D1;
                                if (conversationManager.CurOptions[index].IsClickable)
                                {
                                    conversationManager.DoOption(index);
                                    Debug("Option: " + index);
                                }
                            }
                        }
                    }
                }
            }
            #region DEBUG
            if (debug && Input.IsKeyPressed(debugKey))
            {
                if (conversationManager == null)
                {
                    Debug("No access to ConversationManager");
                }
                else if (!conversationManager.IsConversationInProgress)
                {
                    Debug("No active conversation");
                }
                else
                {
                    Debug("In conversation");
                }
            }
            #endregion
        }

        public override void OnGameLoaded(Game game, object initializerObject)
        {
            Debug("OnGameLoaded");
            conversationManager = Campaign.Current.ConversationManager;
        }

        public override void OnGameEnd(Game game)
        {
            Debug("OnGameEnd");
            conversationManager = null;
        }

        private void Debug(string log)
        {
            if (debug)
            {
                InformationManager.DisplayMessage(new InformationMessage("[DialogueHotkeys] " + log));
            }
        }
    }
}
