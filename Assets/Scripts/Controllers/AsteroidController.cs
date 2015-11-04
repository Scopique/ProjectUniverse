using UnityEngine;
using System.Collections.Generic;

/* =======================================================================
 * Asteroid Controller
 * -------------------
 * Demographic information
 * Spawn point generator
 * 
 * The spawn point generator uses the circular point method to place a
 * number of empty game objects at points arrayed around the center point
 * of the asteroid game object. 
 * 
 * The number of spawn points should always be equal to to greater than 
 * the max number of Miner NPCs in this sector so that TECHNICALLY all 
 * miners COULD spawn at the same asteroid (although that would look really
 * stupid).
 * =====================================================================*/
public class AsteroidController : MonoBehaviour
{

    #region Inspector Variables

    //The ID of the mineral buried in this rock. Currently,
    //  only one mineral per asteroid. 
    public int mineralID;
    //Total number of spawn points to generate
    public float maxSpawnPoints = 6.0f;

    //List of spawn points that have miners assigned to them
    //Populated when the miners actually spawn
    public List<string> egressAssigned;

    #endregion

    #region Local Variables
        
    #endregion

    #region Unity Methods
    

    /// <summary>
    /// Init and call for the creation of spawn points
    /// </summary>
	void Awake () {
        egressAssigned = new List<string>();
        SpawnEgressPoints(3, maxSpawnPoints);
	}
	
    #endregion

    #region Private Methods

    /// <summary>
    /// create a series of empty game objects arrayed around the centeral point
    /// of the Asteroid game object.
    /// </summary>
    /// <param name="radius">How far from the object center to spawn. Bigger values for bigger models</param>
    /// <param name="totalPoints">Total number of spawn points to generate per asteroid. Should be <= # miners in the system</param>
    private void SpawnEgressPoints(float radius, float totalPoints)
    {

        Vector3 center = transform.position;
        float currentPoint = 0;

        for (float i = 1; i <= totalPoints; i++) 
        { 
            float ptRatio = currentPoint / totalPoints;
            float pointX = center.x + (Mathf.Cos(ptRatio * 2 * Mathf.PI)) * radius;
            float pointZ = center.z + (Mathf.Sin(ptRatio * 2 * Mathf.PI)) * radius;
            float pointY = center.y + (Mathf.Tan(ptRatio * 2 * Mathf.PI)) * radius;

            Vector3 spawnPoint = new Vector3(pointX, pointY, pointZ);

            GameObject egressPoint = new GameObject("ast_egressPoint" + currentPoint);
            egressPoint.transform.parent = transform;
            egressPoint.transform.position = spawnPoint;
            egressPoint.tag = "EgressPoint";

            currentPoint++;
        }

    }

    #endregion
}
