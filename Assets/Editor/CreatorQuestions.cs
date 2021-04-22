using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Victorina
{

    class CreatorQuestions : EditorWindow
    {
        private string _question;
        private string _answer1;
        private string _answer2;
        private string _answer3;
        private string _answer4;
        private string _answer5;

       // private TabButton _tabButton;

        [MenuItem("Window/My Window")]

        public static void ShowWindow()
        {
            GetWindow(typeof(CreatorQuestions));
        }

        void OnGUI()
        {
            GUILayout.Label("���������� �������", EditorStyles.boldLabel);
            _question = EditorGUILayout.TextField("������", _question);
            _answer1 = EditorGUILayout.TextField("�����1", _answer1);
            _answer2 = EditorGUILayout.TextField("�����2", _answer2);
            _answer3 = EditorGUILayout.TextField("�����3", _answer3);
            _answer4 = EditorGUILayout.TextField("�����4", _answer4);
            _answer5 = EditorGUILayout.TextField("�����5", _answer5);

           // _tabButton = new TabButton("�������", _tabButton);

            if (GUILayout.Button("�������"))
                AddNewQuestion();
        }

        private void AddNewQuestion()
        {
            string filename = "Assets/Resources/base.txt";

            using var sw = new StreamWriter(filename, true, System.Text.Encoding.Default);
            sw.Write("*\r" + _question + "\r~ " + _answer1 + "\r~ " + _answer2 + "\r~ " + _answer3 + "\r~ " + _answer4 + "\r~ " + _answer5 + "\r");

        }

    }

}