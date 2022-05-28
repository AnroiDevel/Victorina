using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;


namespace Victorina
{
    [System.Serializable]
    public class Character
    {
        public string Name = "Игрок";
        public DateTime CreatedAkkDate;
        public string PlayFabId;
        public string PathToAvatar;
        public float ScaleAvatarCoef = 1.0f;
        public int Money;
        public int Tickets;

        [NonSerialized]
        public Sprite Avatar;

        internal int Bonus;
        internal int BonusSecondsTime;
        internal int WeeklyRank;
        internal int AllQuestionsCount;
        internal string AverageTimeAnswers;
        internal string AllGameTime;
        internal string LastGameTime;
        internal int RightAnswersCount;
        internal int LastGameTimeSec;
        internal int MonthRank;
        internal int HelpicR2;
        internal int HelpicRE;
        internal int BitTicket;
        internal bool IsPlay;
        internal bool IsVip;
        internal bool IsNotReclama;
        internal int PriceBitTicket;
        internal string PlayToken;
        internal DateTime? StartGameTime;
        internal bool IsTrain;
        internal bool RightError;
    }
}