using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class PersonController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public Transform targetWindow;
    public Transform targetperson=null;
    public Transform targetExit = null;

    public bool InService { get; set; }
    public GameObject ATM;
    public QueueManager queueManager;

    public enum PersonState
    {
        None=-1,
        Entered,  //going towards the ATM (don't bump into fron persons)
        InService,
        Serviced
    }
    public PersonState personState = PersonState.None;
    //UI debugging
    public Text txtDebug;
    // Start is called before the first frame update
    void Start()
    {
        ATM = GameObject.FindGameObjectWithTag("ATM");
        targetWindow = ATM.transform;
        targetExit = GameObject.FindGameObjectWithTag("PersonExit").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        txtDebug = GameObject.Find("txtDebug").GetComponent<Text>();
        print("Start: this.GO.ID=" + this.gameObject.GetInstanceID());
        txtDebug.text += "\nStart: this.GO.ID=" + this.gameObject.GetInstanceID();
        //
        personState = PersonState.Entered;
        FSMperson();

    }

    void FSMperson()
    {
        print("CC.FSMperson:state=" + personState+",ID="+this.gameObject.GetInstanceID());
        txtDebug.text += "\nCC.FSMperson:state=" + personState + ",ID=" + this.gameObject.GetInstanceID();
        switch (personState)
        {
            case PersonState.None: //do nothing - shouldn't happen
                break;
            case PersonState.Entered:
                DoEntered();
                break;
            case PersonState.InService:
                DoInService();
                break;
            case PersonState.Serviced:
                DoServiced();
                break;
            default:
                print("personState unknown!:" + personState);
                break;

        }
    }
    void DoEntered()
    {
//        //queueManager = ATM.GetComponent<QueueManager>();
//        GameObject goLast = GameObject.FindGameObjectWithTag("ATM").GetComponent<QueueManager>().Last();
//        if (goLast)
//        {
//#if DEBUG_CC
//            print("CC.DoEntered: goLast.ID=" + goLast.GetInstanceID());
//#endif
//            targetperson = goLast.transform;
//        }
//        else
//        {
//            targetperson = targetWindow;
//        }

        targetperson = targetWindow;

        queueManager = GameObject.FindGameObjectWithTag("ATM").GetComponent<QueueManager>();
        queueManager.Add(this.gameObject);

        navMeshAgent.SetDestination(targetperson.position);
        navMeshAgent.isStopped = false;
    }
    void DoInService()
    {
        navMeshAgent.isStopped = true;
        //this.transform.position = targetWindow.position;
        //this.transform.rotation = Quaternion.identity;
    }
    void DoServiced()
    {
        navMeshAgent.SetDestination(targetExit.position);
        navMeshAgent.isStopped = false;
    }
    public void ChangeState(PersonState newPersonState)
    {
        this.personState = newPersonState;
        FSMperson();
    }

    public void FixedUpdate()
    {

//        if (personState == personState.Entered)
//        {
//            if (targetperson == null)
//            {
//#if DEBUG_CC
//            print("***** personController.FixedUpdate:targetperson.pos=" + targetperson.position);
//#endif
//                targetperson = targetWindow;
//                //navMeshAgent.SetDestination(targetperson.position);
//                navMeshAgent.isStopped = false;
//            }
//        }

    }
    public void SetInService(bool value)
    {
        //Chaneg        InService = value;
        //if (InService)
        //{
        //    navMeshAgent.isStopped=true;
        //}
    }
    public void ExitService(Transform target)
    {
        //this.SetInService(false);
        
        queueManager.PopFirst();
        ChangeState(PersonState.Serviced);
        //targetExit = target;

        //navMeshAgent.SetDestination(target.position);
        //navMeshAgent.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogFormat("CONSOLE: PersonController(this={0}).OnTriggerEnter:other={1}", this.gameObject.GetInstanceID(), other.gameObject.tag);
        txtDebug.text += $"\nPersonController(this={this.gameObject.GetInstanceID()}).OnTriggerEnter:other={other.gameObject.tag}";
        if (other.gameObject.tag == "Person")
        {
            //this.navMeshAgent.desiredVelocity.
            //if (targetperson == null)
            //{
                //targetperson = other.gameObject.transform;
                //navMeshAgent.SetDestination(targetperson.position);
            //}
        }
        else if (other.gameObject.tag == "ATM")
        {
            ChangeState(PersonState.InService);
            //SetInService(true);
        }
        else if (other.gameObject.tag == "PersonExit")
        {
            Destroy(this.gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        print("InCC.OnDrawGizmos:targetperson.ID=" + targetperson.gameObject.GetInstanceID());
        print("InCC.OnDrawGizmos:targetperson.ID=" + targetExit.gameObject.GetInstanceID());
        //txtDebug.text += "\nCONSOLE: InCC.OnDrawGizmos:targetperson.ID=" + targetperson.gameObject.GetInstanceID();
        //txtDebug.text += "\nCONSOLE: InCC.OnDrawGizmos:targetperson.ID=" + targetExit.gameObject.GetInstanceID();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, targetWindow.transform.position);
        if (targetperson)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.position, targetperson.transform.position);

        }
        if (targetExit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position, targetExit.transform.position);

        }


    }

}
