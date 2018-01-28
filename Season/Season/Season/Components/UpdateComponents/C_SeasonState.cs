using Microsoft.Xna.Framework;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    enum eSeason {
        Spring,
        Summer,
        Autumn,
        Winter,
        None,
    }
    class C_SeasonState : UpdateComponent
    {
        private eSeason nowSeason;
        private List<int> skillCD;
        private bool canSkill;
        private Timer skillTimer;
        Rectangle rect;

        public C_SeasonState(eSeason season = eSeason.Spring) {
            nowSeason = season;
            skillCD = new List<int>() { 10, 20, 15, 20, 0 };
            canSkill = true;
            skillTimer = new Timer(skillCD[(int)nowSeason]);
            rect = new Rectangle((int)nowSeason * 48, 0, 48, 48);
        }

        public void CoolingSkill() { canSkill = false; }
        public bool IsCanSkill() { return canSkill; }
        public Rectangle GetRect(eSeason season) { return rect; }
        public Timer GetSkillCD() { return skillTimer; }

        public override void Update() {
            if (!canSkill) {
                skillTimer.Update();
                if (skillTimer.IsTime) {
                    skillTimer.SetTimer(skillCD[(int)nowSeason]);
                    Console.WriteLine(skillCD[(int)nowSeason]);
                    canSkill = true;
                }
            }
        }

        public eSeason GetNowSeason() { return nowSeason; }

        public void ToNextSeason() {
            int index = (int)nowSeason;
            if (index + 1 >= (int)eSeason.None) {
                index = index + 1 - (int)eSeason.None;
            } else {
                index++;
            }
            nowSeason = (eSeason)index;
        }

        public void ToBeforeSeason() {
            int index = (int)nowSeason;
            if (index - 1 < 0) {
                index = index - 1 + (int)eSeason.None;
            } else {
                index--;
            }
            nowSeason = (eSeason)index;
        }

        public void SetNowSeason(eSeason season) { nowSeason = season; }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            skillCD.Clear();
        }

    }
}