using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGameManager : MonoBehaviour
{
    //Script qui regroupe toutes les variables static du jeu qui doivent etre accessibles en tout temps//

    /**********************/
    /* Système et Options */
    /**********************/

    public static byte lang = 1;                //Langue, colone des tableaux de dialogue

    /**********************/
    /*      Coffres       */
    /**********************/

    //Coffres

    public static int[] Coffres = new int[100];
}
