using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IconEditor))]//拡張するクラスを指定
public class IconEditorButton  : Editor
{
    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        //targetを変換して対象を取得
        IconEditor iconEditor = target as IconEditor;

        //PrivateMethodを実行する用のボタン
        if (GUILayout.Button("SaveIcons"))
        {
            //SendMessageを使って実行
            iconEditor.SendMessage("SaveIcons", null, SendMessageOptions.DontRequireReceiver);
        }

    }

}