using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TheDialogueManager : MonoBehaviour
{
    // Regroupe toutes les variables et les fonction de gestion de texte

    // Extrait le texte et le renvoie dans une String
    public static string[,] TextExtractor(TextAsset File)
    {
        string textFile;
        string[] stringTable;
        string[,] TextTable;
        int x = 0;
        int y = 0;
        int l = 0;

        //Recupere le texte du fichier
        if (File == null)
            textFile = "NO FILE";
        else
            textFile = File.ToString();

        //Repaire toute les entrees du texte
        stringTable = textFile.Split(';', '\n');

        //Calcule le nombre de colone et de ligne
        for (int i = 0; i < File.ToString().Length; i++)
        {
            if (textFile[i] == '\n')
                y++;
            if (textFile[i] == ';')
                x++;
        }

        //Declare un tableau selon les ligne et les colones
        TextTable = new string[(x / y) + 1, y];

        //Remplis le tableau avec les entrees de textes
        for (int j = 0; j < y; j++)
        {
            for (int k = 0; k < (x / y) + 1; k++)
            {
                TextTable[k, j] = stringTable[l];
                l++;
            }
        }
        return TextTable;
    }

}
