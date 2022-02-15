using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGameManager : MonoBehaviour
{
    //Script qui regroupe toutes les variables static du jeu qui doivent etre accessibles en tout temps//

    /**********************/
    /* Système et Options */
    /**********************/

    public static byte lang = 1;                // Langue, colone des tableaux de dialogue

    /**********************/
    /*   Teleportation    */
    /**********************/

    public static Vector3 Dest = new Vector3(-12, 7, 0); //Vector3.zero;  // Points d'arrivée dans une autre scene via un teleporteur

    /**********************/
    /*      Coffres       */
    /**********************/

    //Coffres

    public static bool[] Coffres = new bool[100];

    /**********************/
    /*      Events        */
    /**********************/

    //Evenements

    public static bool[] Events = new bool[100];

    /*  Liste des Evenements
     *  
     *  0 = EV_Ship01_01 / map = Ship01
     * 
     * 
     * 
     * 
     */

}
