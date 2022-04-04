// using Domain.Enums;
// using Domain.Models;
// using Engine.Handlers.Interfaces;
// using Engine.Interfaces;
// using Engine.Models;
// using Engine.Services;
//
// namespace Engine.Handlers.Actions
// {
//     public class SendBuilderActionHandler : IActionHandler
//     {
//         private readonly IWorldStateService worldStateService;
//         private readonly EngineConfig engineConfig;
//
//         public SendBuilderActionHandler(IWorldStateService worldStateService, IConfigurationService engineConfig)
//         {
//             this.worldStateService = worldStateService;
//             this.engineConfig = engineConfig.Value;
//         }
//         
//         public bool IsApplicable(PlayerAction action) => action.Action == PlayerActions.Build;
//
//         public void ProcessAction(BotObject bot)
//         {
//             if (!bot.HasAvailableUnit)
//             {
//                 return;
//             }
//             
//             var activeTask = new ActiveTask
//             {
//                 Task = Task.Building,
//                 EffectDuration = engineConfig.Shield.ShieldEffectDuration
//             };
//
//             // If the effect is not registered, add the shield and add it to the list. #1#
//             if (worldStateService.GetActiveEffectByType(bot.Id, Task.Building) == default)
//             {
//                 worldStateService.AddActiveEffect(activeTask);
//             }
//         }
//     }
// }