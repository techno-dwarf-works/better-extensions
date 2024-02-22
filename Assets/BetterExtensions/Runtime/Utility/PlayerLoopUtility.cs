using System;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Better.Extensions
{
    public static class PlayerLoopUtility
    {
        #region Subscribing

        public static void SubscribeToLoop(Type loopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            ref var loopSystem = ref currentSystem.GetSubSystem(loopType);
            loopSystem.updateDelegate += updateFunction;
            PlayerLoop.SetPlayerLoop(currentSystem);
        }

        public static void SubscribeToLoop<TLoop>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var loopType = typeof(TLoop);
            SubscribeToLoop(loopType, updateFunction);
        }

        public static void UnsubscribeFromLoop(Type loopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            ref var loopSystem = ref currentSystem.GetSubSystem(loopType);
            loopSystem.updateDelegate -= updateFunction;
            PlayerLoop.SetPlayerLoop(currentSystem);
        }

        public static void UnsubscribeFromLoop<TLoop>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var loopType = typeof(TLoop);
            UnsubscribeFromLoop(loopType, updateFunction);
        }

        #endregion

        #region Insert

        public static void InsertLoopBefore(Type sourceLoopType, Type destinationLoopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            InsertLoopWithOffset(sourceLoopType, destinationLoopType, 0, updateFunction);
        }

        public static void InsertLoopBefore<TLoopSource, TLoopDestination>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var sourceType = typeof(TLoopSource);
            var destinationType = typeof(TLoopDestination);
            InsertLoopBefore(sourceType, destinationType, updateFunction);
        }

        public static void InsertLoopAfter(Type sourceLoopType, Type destinationLoopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            InsertLoopWithOffset(sourceLoopType, destinationLoopType, 1, updateFunction);
        }

        public static void InsertLoopAfter<TLoopSource, TLoopDestination>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var sourceType = typeof(TLoopSource);
            var destinationType = typeof(TLoopDestination);
            InsertLoopAfter(sourceType, destinationType, updateFunction);
        }

        private static void InsertLoopWithOffset(Type sourceLoopType, Type destinationLoopType, int insertOffset, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            ref var parentSystem = ref currentSystem.GetParentSystemRecursiveOf(sourceLoopType);

            var sourceSubIndex = parentSystem.FindSubSystemIndex(sourceLoopType);
            if (sourceSubIndex < 0)
            {
                var message = $"[{nameof(PlayerLoopUtility)}] {nameof(InsertLoopWithOffset)}: not found {nameof(sourceLoopType)}({sourceLoopType.Name})";
                Debug.LogWarning(message);
                return;
            }

            var destinationSystem = new PlayerLoopSystem
            {
                type = destinationLoopType,
                updateDelegate = updateFunction
            };

            if (parentSystem.subSystemList == null)
            {
                parentSystem.subSystemList = new[] { destinationSystem };
            }
            else if (parentSystem.HasSubSystemOf(destinationLoopType))
            {
                var message = $"[{nameof(PlayerLoopUtility)}] {nameof(InsertLoopWithOffset)}: {nameof(sourceLoopType)}({sourceLoopType.Name}) already contains {nameof(destinationLoopType)}{destinationLoopType}";
                Debug.LogWarning(message);
                return;
            }
            else
            {
                var destinationIndex = sourceSubIndex + insertOffset;
                destinationIndex = Mathf.Clamp(destinationIndex, 0, parentSystem.subSystemList.Length);

                var subSystems = parentSystem.subSystemList.ToList();
                subSystems.Insert(destinationIndex, destinationSystem);
                parentSystem.subSystemList = subSystems.ToArray();
            }

            PlayerLoop.SetPlayerLoop(currentSystem);
        }

        #endregion

        #region Remove

        public static bool Remove(Type loopType)
        {
            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            var anyRemoved = currentSystem.RemoveSubSystemRecursive(loopType);
            if (anyRemoved)
            {
                PlayerLoop.SetPlayerLoop(currentSystem);
            }

            return anyRemoved;
        }

        public static bool Remove<TLoop>()
        {
            var loopType = typeof(TLoop);
            return Remove(loopType);
        }

        #endregion
    }
}