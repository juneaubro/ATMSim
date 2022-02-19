using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrivalProcess : MonoBehaviour
{

    public GameObject personPrefab;
    public Transform personSpawnPlace;

    public float arrivalRateAspersonsPerHour = 20; // person/hour
    public float interArrivalTimeInHours; // = 1.0 / arrivalRateAspersonsPerHour;
    private float interArrivalTimeInMinutes;
    private float interArrivalTimeInSeconds;

    //public float arrivalRateAspersonsPerHour = 20; // person/hour
    public bool generateArrivals = true;

    //New as of Feb.23rd
    //Simple generation distribution - Uniform(min,max)
    //
    public float minInterArrivalTimeInSeconds = 3; 
    public float maxInterArrivalTimeInSeconds = 60;
    //
    public enum ArrivalIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    public ArrivalIntervalTimeStrategy arrivalIntervalTimeStrategy=ArrivalIntervalTimeStrategy.UniformIntervalTime;

    //New as of Feb.25th
    QueueManager queueManager;

    //UI debugging
    public Text txtDebug;

    // Start is called before the first frame update
    void Start()
    {
        queueManager = GameObject.FindGameObjectWithTag("ATM").GetComponent<QueueManager>();
        interArrivalTimeInHours = 1.0f / arrivalRateAspersonsPerHour;
        interArrivalTimeInMinutes = interArrivalTimeInHours * 60;
        interArrivalTimeInSeconds = interArrivalTimeInMinutes * 60;
        StartCoroutine(GenerateArrivals());

        print("proc#:" + System.Environment.ProcessorCount);
        txtDebug.text += "\nproc#:" + System.Environment.ProcessorCount;
    }
   
    IEnumerator GenerateArrivals()
    {
        while (generateArrivals)
        {
            GameObject personGO=Instantiate(personPrefab, personSpawnPlace.position, Quaternion.identity);
            //if (queueManager.Count() > 0)
            //{
            //    queueManager.Add(personGO);
            //} //The first person as added in the queue when in ATM

            float timeToNextArrivalInSec = interArrivalTimeInSeconds;
            switch (arrivalIntervalTimeStrategy)
            {
                case ArrivalIntervalTimeStrategy.ConstantIntervalTime:
                    timeToNextArrivalInSec= interArrivalTimeInSeconds;
                    break;
                case ArrivalIntervalTimeStrategy.UniformIntervalTime:
                    timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds, maxInterArrivalTimeInSeconds);
                    break;
                case ArrivalIntervalTimeStrategy.ExponentialIntervalTime:
                    float U = Random.value;
                    float Lambda = 1 / arrivalRateAspersonsPerHour;
                    timeToNextArrivalInSec = Utilities.GetExp(U,Lambda);
                    break;
                case ArrivalIntervalTimeStrategy.ObservedIntervalTime:
                    timeToNextArrivalInSec = interArrivalTimeInSeconds;
                    break;
                default:
                    print("No acceptable arrivalIntervalTimeStrategy:" + arrivalIntervalTimeStrategy);
                    break;

            }

            //New as of Feb.23rd
            //float timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds,maxInterArrivalTimeInSeconds);
            yield return new WaitForSeconds(timeToNextArrivalInSec);

            //yield return new WaitForSeconds(interArrivalTimeInSeconds);

        }

    }
}
