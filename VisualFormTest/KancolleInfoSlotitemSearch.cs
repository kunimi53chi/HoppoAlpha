using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    static class KancolleInfoSlotitemSearch
    {
        //検索する装備の種類
        public static int SearchType { get; set; }
        //検索する装備のID
        public static int SearchID { get; set; }
        //初期化されているか
        public static bool IsInited { get; set; }
        //敵装備を表示するか
        public static bool ShowEnemySlotitem { get; set; }
        //コールバック
        delegate void SetComboBoxItemCallBack(System.Windows.Forms.ComboBox comboBox, string[] items, int[] tags, int selectedIndex);
        delegate int GetComboBoxSelectedIndexCallBack(System.Windows.Forms.ComboBox comboBox);
        delegate void SetComboBoxSelectedIndexCallBack(System.Windows.Forms.ComboBox comboBox, int index);
        delegate void SetListViewItemsCallBack(System.Windows.Forms.ListView listview, System.Windows.Forms.ListViewItem[] items);

        #region コールバック
        //コンボボックスのアイテム
        private static void SetComboBoxItem(System.Windows.Forms.ComboBox comboBox, string[] items, int[] tags, int selectedIndex)
        {
            if(comboBox.InvokeRequired)
            {
                SetComboBoxItemCallBack d = new SetComboBoxItemCallBack(SetComboBoxItem);
                comboBox.Invoke(d, new object[] { comboBox, items, tags, selectedIndex });
            }
            else
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(items);
                comboBox.Tag = tags;
                if (items.Length > 0) comboBox.SelectedIndex = selectedIndex;
                else comboBox.SelectedIndex = -1;
            }
        }

        private static int GetComboBoxSelectedIndex(System.Windows.Forms.ComboBox comboBox)
        {
            if(comboBox.InvokeRequired)
            {
                GetComboBoxSelectedIndexCallBack d = new GetComboBoxSelectedIndexCallBack(GetComboBoxSelectedIndex);
                return (int)comboBox.Invoke(d, new object[]{comboBox});
            }
            else
            {
                return comboBox.SelectedIndex;
            }
        }

        private static void SetComboBoxSelectedIndex(System.Windows.Forms.ComboBox comboBox, int selectedIndex)
        {
            if(comboBox.InvokeRequired)
            {
                SetComboBoxSelectedIndexCallBack d = new SetComboBoxSelectedIndexCallBack(SetComboBoxSelectedIndex);
                comboBox.Invoke(d, new object[] { comboBox, selectedIndex });
            }
            else
            {
                comboBox.SelectedIndex = selectedIndex;
            }
        }

        //リストビュー
        private static void SetListViewItems(System.Windows.Forms.ListView listview, System.Windows.Forms.ListViewItem[] items)
        {
            if(listview.InvokeRequired)
            {
                SetListViewItemsCallBack d = new SetListViewItemsCallBack(SetListViewItems);
                listview.Invoke(d, new object[] { listview, items });
            }
            else
            {
                listview.Items.Clear();
                listview.Items.AddRange(items);
            }
        }
        #endregion

        //結果表示用のクラス
        public class SearchResult
        {
            public int Index { get; set; }
            public int ID { get; set; }
            public ApiShip Ship { get; set; }
            public SlotItem SlotItem { get; set; }
            public string Page { get; set; }
            public ExMasterSlotitem MasterSlotItem { get; set; }
            public int Num { get; set; }
            public int ReinforcedLevel { get; set; }
            public int TrainingLevel { get; set; }
        }

        //Init
        public static void Init(System.Windows.Forms.Control[] controls)
        {
            //[0] combobox 分類
            //[1] combobox 種別
            //[2] button 検索
            //[3] listview
            //[4] 結果表示
            //[5] ステータス
            //[6] 表示しないチェックボックス

            //イベントハンドラーはForm1側で

            //分類の初期化
            System.Windows.Forms.ComboBox parent = (System.Windows.Forms.ComboBox)controls[0];
            string[] parent_item = new string[APIMaster.MstSlotitemEquiptypes.Count];
            int[] parent_id = new int[parent_item.Length];
            int cnt = 0; 
            foreach(ApiMstSlotitemEquiptype etype in APIMaster.MstSlotitemEquiptypes)
            {
                parent_item[cnt] = etype.api_name;
                parent_id[cnt] = etype.api_id;
                cnt++;
            }
            SetComboBoxItem(parent, parent_item, parent_id, 0);
            //種別の初期化
            System.Windows.Forms.ComboBox sub = (System.Windows.Forms.ComboBox)controls[1];
            int selected = GetComboBoxSelectedIndex(parent);
            SubComboBoxSet(sub, parent_id[selected]);
            //Enabledの設定
            CallBacks.SetControlEnabled(parent, true);
            CallBacks.SetControlEnabled(sub, true);
            CallBacks.SetControlEnabled(controls[2], true);
            CallBacks.SetControlEnabled(controls[5], true);
            CallBacks.SetControlEnabled(controls[6], true);
            //コンボボックスのIDの引き継ぎ
            if(Config.SlotitemSearchMainIndex < parent.Items.Count)
            {
                SetComboBoxSelectedIndex(parent, Config.SlotitemSearchMainIndex);
                SlotitemSearch_ParentComboBox_SelectedIndexChanged(parent, EventArgs.Empty);
                if(Config.SlotitemSearchSubIndex < sub.Items.Count)
                {
                    SetComboBoxSelectedIndex(sub, Config.SlotitemSearchSubIndex);
                    SlotitemSearch_SubComboBox_SelectedIndexChanged(sub, EventArgs.Empty);
                }
            }
            //初期化フラグ
            IsInited = true;
        }

        //種別のコンボボックス
        public static void SubComboBoxSet(System.Windows.Forms.ComboBox comboBox, int parent_etype)
        {
            //api_typeの[2]が種別
            var query = from e in APIMaster.MstSlotitems.Values
                        where !e.IsEnemySlotitem || ShowEnemySlotitem
                        where e.api_type != null && e.EquipType == parent_etype
                        select e;
            int n = query.Count();
            string[] sub_item = new string[n+1];
            int[] sub_id = new int[n+1];
            sub_item[0] = "全て"; sub_id[0] = -1;
            int cnt = 1;
            foreach(var x in query)
            {
                sub_item[cnt] = x.api_name;
                sub_id[cnt] = x.api_id;
                cnt++;
            }
            SetComboBoxItem(comboBox, sub_item, sub_id, 0);
            //選択されているID
            SearchType = parent_etype;
            if (query.Count() == 0) SearchID = -1;
            else SearchID = sub_id[0];
        }

        //検索コマンド
        public static void Search(System.Windows.Forms.ListView listview, System.Windows.Forms.Label label_result)
        {
            if (SearchID == -1 && SearchType == -1) return;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //対象の装備アイテム
            var target_item = new List<ExMasterSlotitem>();
            if(SearchID == -1)
            {
                //カテゴリ内全て表示の場合
                target_item = APIMaster.MstSlotitems.Values.Where(x => x.api_type != null && x.EquipType == SearchType).ToList();
            }
            else
            {
                //特定アイテムの場合
                target_item.Add(APIMaster.MstSlotitems[SearchID]);
            }

            //クエリ
            List<SearchResult> query = new List<SearchResult>();
            //Unsetslot
            foreach (var unsets in APIGetMember.Unsetslots.slottype)
            {
                foreach (int x in unsets)
                {
                    SlotItem slot;
                    if (APIGetMember.SlotItemsDictionary.TryGetValue(x, out slot))
                    {
                        foreach (var t in target_item)
                        {
                            if (slot.api_slotitem_id == t.api_id)
                            {
                                SearchResult item = new SearchResult()
                                {
                                    ID = x,
                                    Index = query.Count,
                                    Ship = null,
                                    SlotItem = slot,
                                    Page = null,
                                    MasterSlotItem = t,
                                    Num = 1,
                                    ReinforcedLevel = slot.api_level,
                                    TrainingLevel = slot.api_alv,
                                };
                                query.Add(item);
                            }
                        }
                    }
                }
            }

            //--船がどこに位置しているかを記録しておく
            Dictionary<int, string> shippage = new Dictionary<int, string>();
            //艦隊所属艦
            foreach(int i in Enumerable.Range(0, APIPort.DeckPorts.Count))
            {
                foreach(int j in Enumerable.Range(0, APIPort.DeckPorts[i].api_ship.Count))
                {
                    int shipid = APIPort.DeckPorts[i].api_ship[j];
                    if(shipid == -1) break;
                    shippage[shipid] = string.Format("第{0}-{1}", i+1,j+1);
                }
            }
            var assigns = shippage.Keys.ToArray();//所属している艦
            //改装の他のソートのシミュレート
            var othership = APIPort.ShipsDictionary.Values
                .Where(x => Array.IndexOf(assigns, x.api_id) == -1)//艦隊所属ではなく
                .OrderBy(x => x.api_lv)//レベル昇順
                .ThenByDescending(x => x.DShip.api_sortno)//api_sortno降順
                .ThenByDescending(x => x.api_id);//api_id降順
            //非所属艦の位置の記憶
            int notassigncnt = 0;
            foreach(var x in othership)
            {
                shippage[x.api_id] = string.Format("p{0}-{1}", notassigncnt / 10 + 1, notassigncnt % 10 + 1);
                notassigncnt++;
            }

            //--船
            foreach(ApiShip s in APIPort.ShipsDictionary.Values)
            {
                //通常のスロット
                for(int i=0; i<s.api_slot.Count; i++)
                {
                    int id = s.api_slot[i];
                    //装備していない場合
                    if(id == -1)
                    {
                        break;
                    }
                    else
                    {
                        //キーが存在するかチェック
                        SlotItem slot;
                        if (!APIGetMember.SlotItemsDictionary.TryGetValue(id, out slot)) continue;
                        //見つかった場合
                        foreach (var t in target_item)
                        {
                            if (slot.api_slotitem_id == t.api_id)
                            {
                                SearchResult item = new SearchResult()
                                {
                                    ID = id,
                                    Index = query.Count,
                                    Ship = s,
                                    SlotItem = slot,
                                    MasterSlotItem = t,
                                    Num = 1,
                                    ReinforcedLevel = slot.api_level,
                                    TrainingLevel = slot.api_alv,
                                };
                                string page;
                                if (shippage.TryGetValue(s.api_id, out page)) item.Page = page;
                                query.Add(item);
                            }
                        }
                    }
                }
                //拡張スロット
                if (s.api_slot_ex <= 0) continue;//装備していない場合
                else
                {
                    SlotItem exslot;
                    if(APIGetMember.SlotItemsDictionary.TryGetValue(s.api_slot_ex, out exslot))
                    {
                        foreach (var t in target_item)
                        {
                            if (exslot.api_slotitem_id == t.api_id)
                            {
                                //拡張スロットに見つかった場合
                                SearchResult exresult = new SearchResult()
                                {
                                    ID = s.api_slot_ex,
                                    Index = query.Count,
                                    Ship = s,
                                    SlotItem = exslot,
                                    MasterSlotItem = t,
                                    Num = 1,
                                    ReinforcedLevel = exslot.api_level,
                                    TrainingLevel = exslot.api_alv,
                                };
                                string page;
                                if (shippage.TryGetValue(s.api_id, out page)) exresult.Page = page;
                                query.Add(exresult);
                            }
                        }
                    }
                }
            }
            //全ての検索の場合検索結果を統合する
            if(SearchID == -1)
            {
                var allresult = new Dictionary<int, SearchResult>();
                foreach(var r in query.OrderBy(x => x.MasterSlotItem.api_id))
                {
                    SearchResult item;
                    if(allresult.TryGetValue(r.MasterSlotItem.api_id, out item))
                    {
                        item.ID = Math.Min(item.ID, r.ID);
                        item.Num += r.Num;
                        item.ReinforcedLevel += r.ReinforcedLevel;
                        item.TrainingLevel += r.TrainingLevel;
                    }
                    else
                    {
                        item = new SearchResult()
                        {
                            ID = r.ID,
                            Index = allresult.Count,
                            MasterSlotItem = r.MasterSlotItem,
                            Num = 1,
                            ReinforcedLevel = r.ReinforcedLevel,
                            TrainingLevel = r.TrainingLevel,
                        };
                    }
                    allresult[item.MasterSlotItem.api_id] = item;
                }

                query = allresult.Values.ToList();
            }

            //リストビューアイテムの作成
            System.Windows.Forms.ListViewItem[] listitem = new System.Windows.Forms.ListViewItem[query.Count];
            int cnt = 0;
            foreach(SearchResult result in query.OrderBy(x => x.MasterSlotItem.api_id))
            {
                System.Windows.Forms.ListViewItem item;

                //改修レベル
                string itemlevel = "";
                if (result.ReinforcedLevel > 0) itemlevel += "★" + result.ReinforcedLevel;
                if (result.TrainingLevel > 0) itemlevel += "◆" + result.TrainingLevel;

                if (result.Ship != null)
                {
                    //個別検索で装備している場合
                    string str;
                    if(result.Page != null)
                    {
                        str = string.Format("{0}(Lv{1})/{2}", result.Ship.ShipName, result.Ship.api_lv, result.Page);
                    }
                    else
                    {
                        str = string.Format("{0}(Lv{1})", result.Ship.ShipName, result.Ship.api_lv);
                    }
                    item = new System.Windows.Forms.ListViewItem(new string[]
                    {
                        result.Index.ToString(), itemlevel, 
                        str,
                        result.MasterSlotItem.api_name, result.ID.ToString(),
                    });
                }
                else
                {
                    //個別検索で装備していない場合
                    if (SearchID != -1)
                    {
                        item = new System.Windows.Forms.ListViewItem(new string[]
                        {
                            result.Index.ToString(), itemlevel, "未装備",
                            result.MasterSlotItem.api_name, result.ID.ToString(),
                        });
                    }
                    //全体検索の場合
                    else
                    {
                        var num = result.Num + "個";
                        if(result.ReinforcedLevel > 0 || result.TrainingLevel > 0)
                        {
                            num += "(平均:";
                            if (result.ReinforcedLevel > 0) num += "★" + ((double)result.ReinforcedLevel / (double)result.Num).ToString("F1");
                            if (result.ReinforcedLevel > 0 && result.TrainingLevel > 0) num += ", ";
                            if (result.TrainingLevel > 0) num += "◆" + ((double)result.TrainingLevel / (double)result.Num).ToString("F1");
                            num += ")";
                        }

                        item = new System.Windows.Forms.ListViewItem(new string[]
                        {
                            result.Index.ToString(), itemlevel, num,
                            result.MasterSlotItem.api_name, result.ID.ToString(),
                        });
                        item.Tag = result.MasterSlotItem;
                    }
                }

                //ToolTip
                item.ToolTipText = string.Format("{0}({1})", result.MasterSlotItem.api_name, itemlevel);

                listitem[cnt] = item;
                cnt++;
            }
            //リストビューにセット
            SetListViewItems(listview, listitem);
            sw.Stop();
            //ラベルに結果を表示
            CallBacks.SetLabelText(label_result, string.Format("{0} 件 ヒットしました - {1}ms", query.Count, sw.Elapsed.TotalMilliseconds.ToString("F0")));
        }

        //イベントハンドラー
        #region イベントハンドラー
        public static void SlotitemSearch_ParentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox parent = (System.Windows.Forms.ComboBox)sender;
            int parentSelected = GetComboBoxSelectedIndex(parent);
            int etype = ((int[])parent.Tag)[parentSelected];
            System.Windows.Forms.ComboBox sub = (System.Windows.Forms.ComboBox)
                DockingWindows.DockWindowTabCollection.GetCollection(parent).EquipSearch.control_eqsearch[1];
            KancolleInfoSlotitemSearch.SubComboBoxSet(sub, etype);
            Config.SlotitemSearchMainIndex = parentSelected;
            SearchType = etype;
        }

        public static void SlotitemSearch_SubComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox sub = (System.Windows.Forms.ComboBox)sender;
            int slotid;
            int subSelected = GetComboBoxSelectedIndex(sub);
            if (subSelected == -1) slotid = -1;
            else slotid = ((int[])sub.Tag)[subSelected];
            KancolleInfoSlotitemSearch.SearchID = slotid;
            Config.SlotitemSearchSubIndex = subSelected;
        }

        public static void SlotitemSearch_Button_Click(object sender, EventArgs e)
        {
            if (APIPort.ShipsDictionary == null) return;
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
            System.Windows.Forms.Control[] control = DockingWindows.DockWindowTabCollection.GetCollection(button).EquipSearch.control_eqsearch;
            System.Windows.Forms.ListView listview = control[3] as System.Windows.Forms.ListView;
            System.Windows.Forms.Label label = control[4] as System.Windows.Forms.Label;
            KancolleInfoSlotitemSearch.Search(listview, label);
        }

        public static void SlotitemSearch_ShowButton_Click(object sender, EventArgs e)
        {
            if (KancolleInfoSlotitemSearch.SearchID == -1) return;
            SlotitemViewer view = new SlotitemViewer();
            ApiMstSlotitem item = APIMaster.MstSlotitems[KancolleInfoSlotitemSearch.SearchID];
            view.Init(item);
            view.Show();
        }

        public static void SlotitemSearch_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox cb = (System.Windows.Forms.CheckBox)sender;
            KancolleInfoSlotitemSearch.ShowEnemySlotitem = !cb.Checked;
            System.Windows.Forms.ComboBox combobox_main = (System.Windows.Forms.ComboBox)
                DockingWindows.DockWindowTabCollection.GetCollection(cb).EquipSearch.control_eqsearch[0];
            //親コンボボックスが変更されたときのイベントと同じ
            SlotitemSearch_ParentComboBox_SelectedIndexChanged(combobox_main, new EventArgs());
        }

        public static void SlotitemSearch_Listview_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listview = (System.Windows.Forms.ListView)sender;
            var first = listview.SelectedItems.OfType<System.Windows.Forms.ListViewItem>().FirstOrDefault();
            if (first == null || first.Tag == null) return;
            //クリックした装備
            var slotitem = (ExMasterSlotitem)first.Tag;
            //子コンボボックス
            var combo_sub = (System.Windows.Forms.ComboBox)DockingWindows.DockWindowTabCollection.GetCollection(listview).EquipSearch.control_eqsearch[1];
            var index = Array.IndexOf((int[])combo_sub.Tag, slotitem.api_id);
            if (index != -1) SetComboBoxSelectedIndex(combo_sub, index);
        }
        #endregion
    }
}
