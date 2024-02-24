using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.LowLevel;

namespace Better.Extensions.Runtime
{
    // TODO: Add summary for possible result self-ref
    public static class PlayerLoopSystemExtensions
    {
        public static bool HasSubSystemOf(this ref PlayerLoopSystem loopSystem, Type subSystemType)
        {
            var index = loopSystem.FindSubSystemIndex(subSystemType);
            return index != -1;
        }

        public static bool HasSubSystemOf<T>(this ref PlayerLoopSystem loopSystem)
        {
            var subSystemType = typeof(T);
            return loopSystem.HasSubSystemOf(subSystemType);
        }

        public static int FindSubSystemIndex(this ref PlayerLoopSystem loopSystem, Type subSystemType)
        {
            var subSystems = loopSystem.subSystemList;
            if (subSystems == null)
            {
                return -1;
            }

            for (int i = 0; i < subSystems.Length; i++)
            {
                if (subSystems[i].type == subSystemType)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int FindSubSystemIndex<T>(this ref PlayerLoopSystem loopSystem)
        {
            var subSystemType = typeof(T);
            return loopSystem.FindSubSystemIndex(subSystemType);
        }

        public static ref PlayerLoopSystem GetSubSystem(this ref PlayerLoopSystem loopSystem, Type subSystemType)
        {
            var index = loopSystem.FindSubSystemIndex(subSystemType);
            if (index == -1)
            {
                return ref loopSystem;
            }

            return ref loopSystem.subSystemList[index];
        }

        public static ref PlayerLoopSystem GetSubSystem<TSubSystem>(this ref PlayerLoopSystem loopSystem)
        {
            var subSystemType = typeof(TSubSystem);
            return ref loopSystem.GetSubSystem(subSystemType);
        }

        public static ref PlayerLoopSystem GetSubSystemRecursive(this ref PlayerLoopSystem loopSystem, Type subSystemType)
        {
            var subSystems = loopSystem.subSystemList;
            if (subSystems == null)
            {
                return ref loopSystem;
            }

            ref var subSystem = ref loopSystem.GetSubSystem(subSystemType);
            if (subSystem.type == subSystemType)
            {
                return ref subSystem;
            }

            for (int i = 0; i < subSystems.Length; i++)
            {
                ref var buffer = ref GetSubSystemRecursive(ref subSystems[i], subSystemType);
                if (buffer.type == subSystemType)
                {
                    return ref buffer;
                }
            }

            return ref loopSystem;
        }

        public static ref PlayerLoopSystem GetSubSystemRecursive<TSubSystem>(this ref PlayerLoopSystem loopSystem)
        {
            var subSystemType = typeof(TSubSystem);
            return ref loopSystem.GetSubSystemRecursive(subSystemType);
        }

        public static ref PlayerLoopSystem GetParentSystemRecursiveOf(this ref PlayerLoopSystem loopSystem, Type subSystemType)
        {
            if (loopSystem.type == subSystemType)
            {
                return ref loopSystem;
            }

            var subSystems = loopSystem.subSystemList;
            if (subSystems == null)
            {
                return ref loopSystem;
            }

            if (loopSystem.HasSubSystemOf(subSystemType))
            {
                return ref loopSystem;
            }

            for (int i = 0; i < subSystems.Length; i++)
            {
                ref var buffer = ref subSystems[i];
                buffer = ref buffer.GetParentSystemRecursiveOf(subSystemType);
                if (buffer.HasSubSystemOf(subSystemType))
                {
                    return ref buffer;
                }
            }

            return ref loopSystem;
        }

        public static ref PlayerLoopSystem GetParentSystemRecursiveOf<TSubSystem>(this ref PlayerLoopSystem loopSystem)
        {
            var subSystemType = typeof(TSubSystem);
            return ref loopSystem.GetParentSystemRecursiveOf(subSystemType);
        }

        public static bool RemoveSubSystem(this ref PlayerLoopSystem loopSystem, Type subSystemType)
        {
            if (loopSystem.subSystemList == null)
            {
                return false;
            }

            var anyRemoved = false;
            var subSystems = loopSystem.subSystemList.ToList();
            for (var i = subSystems.Count - 1; i >= 0; i--)
            {
                if (subSystems[i].type == subSystemType)
                {
                    subSystems.RemoveAt(i);
                    anyRemoved = true;
                }
            }

            if (anyRemoved)
            {
                loopSystem.subSystemList = subSystems.ToArray();
            }

            return anyRemoved;
        }

        public static bool RemoveSubSystem<TSubSystem>(this ref PlayerLoopSystem loopSystem)
        {
            var subSystemType = typeof(TSubSystem);
            return loopSystem.RemoveSubSystem(subSystemType);
        }

        public static bool RemoveSubSystemRecursive(this ref PlayerLoopSystem loopSystem, Type subSystemType)
        {
            var removeAny = loopSystem.RemoveSubSystem(subSystemType);
            var subSystems = loopSystem.subSystemList;
            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].RemoveSubSystemRecursive(subSystemType))
                    {
                        removeAny = true;
                    }
                }
            }

            return removeAny;
        }

        public static bool RemoveSubSystemRecursive<TSubSystem>(this ref PlayerLoopSystem loopSystem)
        {
            var subSystemType = typeof(TSubSystem);
            return loopSystem.RemoveSubSystemRecursive(subSystemType);
        }

        public static bool UnsubscribeRecursive(this ref PlayerLoopSystem loopSystem, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var result = false;
#if UNITY_EDITOR
            var invocationList = loopSystem.updateDelegate.GetInvocationList();
            if (invocationList.Contains(updateFunction))
            {
                loopSystem.updateDelegate -= updateFunction;
                result = true;
            }
#else
                    loopSystem.updateDelegate -= updateFunction;
                    result = true;
#endif

            var subSystems = loopSystem.subSystemList;
            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].UnsubscribeRecursive(updateFunction))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool UnsubscribeRecursive(this ref PlayerLoopSystem loopSystem, Type loopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var result = false;
            if (loopSystem.type == loopType)
            {
#if UNITY_EDITOR
                var invocationList = loopSystem.updateDelegate.GetInvocationList();
                if (invocationList.Contains(updateFunction))
                {
                    loopSystem.updateDelegate -= updateFunction;
                    result = true;
                }
#else
                    loopSystem.updateDelegate -= updateFunction;
                    result = true;
#endif
            }

            var subSystems = loopSystem.subSystemList;
            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].UnsubscribeRecursive(loopType, updateFunction))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool UnsubscribeRecursive<TLoop>(this ref PlayerLoopSystem loopSystem, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var loopType = typeof(TLoop);
            return loopSystem.UnsubscribeRecursive(loopType, updateFunction);
        }
    }
}