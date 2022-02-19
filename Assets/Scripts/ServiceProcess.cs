using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//New as of Feb.25rd

public class ServiceProcess : MonoBehaviour
{
    public GameObject personInService;
    public Transform personExitPlace;

    public float serviceRateAspersonsPerHour = 25; // person/hour
    public float interServiceTimeInHours; // = 1.0 / ServiceRateAspersonsPerHour;
    private float interServiceTimeInMinutes;
    private float interServiceTimeInSeconds;

    //public float ServiceRateAspersonsPerHour = 20; // person/hour
    public bool generateServices = false;

    //New as of Feb.23rd
    //Simple generation distribution - Uniform(min,max)
    //
    public float minInterServiceTimeInSeconds = 3;
    public float maxInterServiceTimeInSeconds = 60;
    //

    //New as Feb.25th
    //personController personController;
    QueueManager queueManager; //=new QueueManager();

    //UI debugging
    public Text txtDebug;

    public enum ServiceIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    public ServiceIntervalTimeStrategy serviceIntervalTimeStrategy = ServiceIntervalTimeStrategy.UniformIntervalTime;

    // Start is called before the first frame update
    void Start()
    {
        interServiceTimeInHours = 1.0f / serviceRateAspersonsPerHour;
        interServiceTimeInMinutes = interServiceTimeInHours * 60;
        interServiceTimeInSeconds = interServiceTimeInMinutes * 60;
        //queueManager = this.GetComponent<QueueManager>();
        //queueManager = new QueueManager();
        //StartCoroutine(GenerateServices());
    }
    private void OnTriggerEnter(Collider other)
    {
        print("ServiceProcess.OnTriggerEnter:otherID=" + other.gameObject.GetInstanceID());
        txtDebug.text += "\nServiceProcess.OnTriggerEnter:otherID=" + other.gameObject.GetInstanceID();
        if (other.gameObject.tag == "Person")
        {
            personInService = other.gameObject;
            personInService.GetComponent<PersonController>().SetInService(true);

            //if (queueManager.Count() == 0)
            //{
            //    queueManager.Add(personInService);
            //}
            
            generateServices = true;
            //personController = personInService.GetComponent<personController>();
            StartCoroutine(GenerateServices());
        }
    }

    IEnumerator GenerateServices()
    {
        while (generateServices)
        {
            //Instantiate(personPrefab, personSpawnPlace.position, Quaternion.identity);
            float timeToNextServiceInSec = interServiceTimeInSeconds;
            switch (serviceIntervalTimeStrategy)
            {
                case ServiceIntervalTimeStrategy.ConstantIntervalTime:
                    timeToNextServiceInSec = interServiceTimeInSeconds;
                    break;
                case ServiceIntervalTimeStrategy.UniformIntervalTime:
                    timeToNextServiceInSec = Random.Range(minInterServiceTimeInSeconds, maxInterServiceTimeInSeconds);
                    break;
                case ServiceIntervalTimeStrategy.ExponentialIntervalTime:
                    float U = Random.value;
                    float Lambda = 1 / serviceRateAspersonsPerHour;
                    timeToNextServiceInSec = Utilities.GetExp(U, Lambda);
                    break;
                case ServiceIntervalTimeStrategy.ObservedIntervalTime:
                    timeToNextServiceInSec = interServiceTimeInSeconds;
                    break;
                default:
                    print("No acceptable ServiceIntervalTimeStrategy:" + serviceIntervalTimeStrategy);
                    break;

            }

            //New as of Feb.23rd
            //float timeToNextServiceInSec = Random.Range(minInterServiceTimeInSeconds,maxInterServiceTimeInSeconds);
            generateServices = false;
            yield return new WaitForSeconds(timeToNextServiceInSec);

            //yield return new WaitForSeconds(interServiceTimeInSeconds);

        }
        personInService.GetComponent<PersonController>().ExitService(personExitPlace);

    }
    private void OnDrawGizmos()
    {
        //BoxColliderpersonInService.GetComponent<BoxCollider>
        if (personInService)
        {
            Renderer r = personInService.GetComponent<Renderer>();
            r.material.color = Color.green;

        }


    }

}
