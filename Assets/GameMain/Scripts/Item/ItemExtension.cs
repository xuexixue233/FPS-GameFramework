using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using GameFramework.DataTable;

namespace FPS
{
    public static class ItemExtension
    {
        private static int s_SerialId = 345;

        public static void ShowItemModUI(this ItemComponent itemComponent)
        {
            itemComponent.ShowItem<ItemModUI>(GenerateSerialId(itemComponent),EnumItem.ItemModUI,"ItemModUI",Constant.AssetPriority.ItemModUIAsset);
        }

        #region ShowItem

        public static void ShowItem(this ItemComponent itemComponent, int serialId, EnumItem enumItem, string itemGroup, int priority, object userData = null)
        {
            itemComponent.ShowItem(serialId, enumItem, null, itemGroup, priority, userData);
        }

        public static void ShowItem<T>(this ItemComponent itemComponent, int serialId, EnumItem enumItem, string itemGroup, int priority, object userData = null)
        {
            itemComponent.ShowItem(serialId, enumItem, typeof(T), itemGroup, priority, userData);
        }

        public static void ShowItem(this ItemComponent itemComponent, int serialId, EnumItem enumItem, Type logicType, string itemGroup, int priority, object userData = null)
        {
            itemComponent.ShowItem(serialId, (int)enumItem, logicType, itemGroup, priority, userData);
        }

        public static void ShowItem(this ItemComponent itemComponent, int serialId, int itemId, string itemGroup, int priority, object userData = null)
        {
            itemComponent.ShowItem(serialId, itemId, null, itemGroup, priority, userData);
        }

        public static void ShowItem<T>(this ItemComponent itemComponent, int serialId, int itemId, string itemGroup, int priority, object userData = null)
        {
            itemComponent.ShowItem(serialId, itemId, typeof(T), itemGroup, priority, userData);
        }

        public static void ShowItem(this ItemComponent itemComponent, int serialId, int itemId, Type logicType, string itemGroup, int priority, object userData = null)
        {
            IDataTable<DRItem> dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
            DRItem drItem = dtItem.GetDataRow(itemId);

            if (drItem == null)
            {
                Log.Warning("Can not load item id '{0}' from data table.", itemId.ToString());
                return;
            }

            itemComponent.ShowItem(serialId, logicType, AssetUtility.GetItemAsset(drItem.Name), itemGroup, priority, userData);
        }

        #endregion


        public static int GenerateSerialId(this ItemComponent itemComponent)
        {
            return ++s_SerialId;
        }
    }
}