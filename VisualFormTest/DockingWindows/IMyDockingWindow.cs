using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualFormTest.DockingWindows
{
    public interface IMyDockingWindow
    {
        int RealWidth { get;  }
        int RealHeight { get; }

        void Stretch();
    }

    public enum NonTabPageType
    {
        Browser, SankWarning, BattleState, TimerViewer, SlotitemNum, 
        BattleDetail, BattleDetailSquare, CompactScreen, DropAnalyzer,
        ToolBox, MapInfo, QuestViewer, BattleDetailSquare2, SFleet,
        SMaterial, SSenka, SortieReportViewer, RankingViewer, CompactScreenVertical,
        KCVDBLog, PresetDeck,
    }

    public static class NonTabPageExt
    {
        public static IMyDockingWindow CreateWindowInstance(this NonTabPageType pagetype, Form1 parent, DockWindowTabCollection collection)
        {
            switch(pagetype)
            {
                case NonTabPageType.Browser:
                    return new DockWindowHoppoBrowser(parent);
                case NonTabPageType.SankWarning:
                    return new DockWindowSankWarning(parent);
                case NonTabPageType.BattleState:
                    return new DockWindowBattleState(parent);
                case NonTabPageType.TimerViewer:
                    return new DockWindowTimerViewer(parent);
                case NonTabPageType.SlotitemNum:
                    return new DockWindowShipSlotitemNum(parent);
                case NonTabPageType.BattleDetail:
                    return new DockWindowBattleDetail(parent);
                case NonTabPageType.BattleDetailSquare:
                    return new DockWindowBattleDetailSquare(parent);
                case NonTabPageType.BattleDetailSquare2:
                    return new DockWindowBattleDetailSquare2(parent);
                case NonTabPageType.CompactScreen:
                    return new DockWindowCompactScreen(parent);
                case NonTabPageType.DropAnalyzer:
                    return new DockWindowDropAnalyzer(parent);
                case NonTabPageType.ToolBox:
                    return new DockWindowToolBox(parent);
                case NonTabPageType.MapInfo:
                    return new DockWindowMapInfo(parent);
                case NonTabPageType.QuestViewer:
                    return new DockWindowQuestViewer(parent);
                case NonTabPageType.SFleet:
                    return new DockWindowShortTabFleet(parent, collection);
                case NonTabPageType.SMaterial:
                    return new DockWindowShortTabMaterial(parent);
                case NonTabPageType.SSenka:
                    return new DockWindowShortTabSenka(parent);
                case NonTabPageType.SortieReportViewer:
                    return new DockWindowSortieReportViewer(parent);
                case NonTabPageType.RankingViewer:
                    return new DockWindowRankingViewer(parent);
                case NonTabPageType.CompactScreenVertical:
                    return new DockWindowCompactScreenVertical(parent);
                case NonTabPageType.KCVDBLog:
                    return new DockWindowKCVDBLog(parent);
                case NonTabPageType.PresetDeck:
                    return new DockWindowPresetDeckViewer(parent);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static NonTabPageType GetEnumFromString(string str)
        {
            switch(str)
            {
                case "DockWindowHoppoBrowser":
                    return NonTabPageType.Browser;
                case "DockWindowSankWarning":
                    return NonTabPageType.SankWarning;
                case "DockWindowBattleState":
                    return NonTabPageType.BattleState;
                case "DockWindowTimerViewer":
                    return NonTabPageType.TimerViewer;
                case "DockWindowShipSlotitemNum":
                    return NonTabPageType.SlotitemNum;
                case "DockWindowBattleDetail":
                    return NonTabPageType.BattleDetail;
                case "DockWindowBattleDetailSquare":
                    return NonTabPageType.BattleDetailSquare;
                case "DockWindowBattleDetailSquare2":
                    return NonTabPageType.BattleDetailSquare2;
                case "DockWindowCompactScreen":
                    return NonTabPageType.CompactScreen;
                case "DockWindowDropAnalyzer":
                    return NonTabPageType.DropAnalyzer;
                case "DockWindowToolBox":
                    return NonTabPageType.ToolBox;
                case "DockWindowMapInfo":
                    return NonTabPageType.MapInfo;
                case "DockWindowQuestViewer":
                    return NonTabPageType.QuestViewer;
                case "DockWindowShortTabFleet":
                    return NonTabPageType.SFleet;
                case "DockWindowShortTabMaterial":
                    return NonTabPageType.SMaterial;
                case "DockWindowShortTabSenka":
                    return NonTabPageType.SSenka;
                case "DockWindowSortieReportViewer":
                    return NonTabPageType.SortieReportViewer;
                case "DockWindowRankingViewer":
                    return NonTabPageType.RankingViewer;
                case "DockWindowCompactScreenVertical":
                    return NonTabPageType.CompactScreenVertical;
                case "DockWindowKCVDBLog":
                    return NonTabPageType.KCVDBLog;
                case "DockWindowPresetDeckViewer":
                    return NonTabPageType.PresetDeck;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
