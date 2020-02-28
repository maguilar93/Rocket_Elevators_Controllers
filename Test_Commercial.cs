using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HelloWorld
{
    static class Global
    {
        static public int ElevPerColumn = 5;
        static public int ColumnPerBattery = 4;
    }

    public class Battery
    {
        public bool status;
        public List<Column> ColumnList { get; set; }

        public Battery(bool status)
        {
            this.status = true;
            this.ColumnList = new List<Column>();
            this.ColumnList.Add(new Column(1, 6, 1));
            this.ColumnList.Add(new Column(8, 26, 2));
            this.ColumnList.Add(new Column(27, 46, 31));
            this.ColumnList.Add(new Column(47, 66, 4));

        }

        public bool RequestColumn(int requestedFloor, string direction, int floorRequest)
        {

            foreach (Column col in ColumnList)
            {

                if ((requestedFloor >= col.startFloor && requestedFloor <= col.endFloor))
                {
                    col.RequestElevator(requestedFloor, direction, floorRequest);
                    Console.WriteLine("In this case, we will use the column #" + col.id);

                    return true;
                }
                else if (requestedFloor == 7)
                {
                    col.RequestElevator(requestedFloor, direction, floorRequest);
                    //Console.WriteLine("Enter the floor you want to go to: ");
                    //string floor = Console.ReadLine();
                    return true;
                }

            }

            return true;
        }
    }

    public class Column
    {
        public int startFloor, endFloor, id;
        public int NbFloors { get; set; }
        public List<Elevator> ListElevators { get; set; }

        public Column(int startFloor, int endFloor, int id)
        {
            this.startFloor = startFloor;
            this.endFloor = endFloor;
            this.id = id;
            NbFloors = endFloor - startFloor;
            this.ListElevators = new List<Elevator>();

            for (var i = 0; i < Global.ElevPerColumn; i++)
            {
                this.ListElevators.Add(new Elevator(1, i + 1));
            }
        }


        public bool RequestElevator(int requestedFloor, string direction, int floorRequest)
        {

            foreach (Elevator elev in ListElevators)
            {
                elev.Gap = Math.Abs(requestedFloor - elev.Current_position);
                elev.Distance = Math.Abs(requestedFloor + elev.Gap);
            }

            IEnumerable<Elevator> query = ListElevators.OrderBy(elev => elev.Gap);
            var orderedList = query.ToList();
            foreach (Elevator elev in orderedList)
            {
                Console.WriteLine(elev.id);
            }


            for (var i = 0; i < orderedList.Count(); i++)
            {
                var curCage = orderedList[i];

                if (curCage.Current_position == requestedFloor && direction == "up" && curCage.Movement == "up")
                {
                    curCage.RequestFloor(floorRequest);
                    Console.WriteLine("hey!");
                    Console.WriteLine("I am sending the elevator # " + curCage.id);
                    return true;
                }
                else if (curCage.Current_position == requestedFloor && curCage.Movement == "idle")
                {
                    curCage.RequestFloor(floorRequest);
                    Console.WriteLine("lol");
                    Console.WriteLine("I am sending the elevator # " + curCage.id);
                    return true;
                }
                else if (curCage.Current_position == requestedFloor && direction == "down" && curCage.Movement == "down")
                {
                    curCage.RequestFloor(floorRequest);
                    Console.WriteLine("hey-");
                    Console.WriteLine("I am sending the elevator # " + curCage.id);
                    return true;
                }
                else if (curCage.Movement == "up" && direction == "up" && curCage.Current_position < requestedFloor)
                {
                    curCage.RequestFloor(floorRequest);
                    Console.WriteLine("ok");
                    Console.WriteLine("I am sending the elevator # " + curCage.id);
                    return true;
                }
                else if (curCage.Movement == "down" && direction == "down" && curCage.Current_position > requestedFloor)
                {
                    curCage.RequestFloor(floorRequest);
                    Console.WriteLine("yes");
                    Console.WriteLine("I am sending the elevator # " + curCage.id);
                    return true;
                }
                else
                {
                    if (curCage.Movement == "down" && direction == "up" && curCage.Current_position < requestedFloor)
                    {


                    }
                }
            }
        }
            return true;
        }


}


public class Elevator
{
    public int position, id;
    public string Movement { get; set; }
    public List<int> Requests { get; set; }
    public bool Obstacles { get; set; }
    public bool OpenDoors { get; set; }
    public int Current_position { get; set; }
    public int Gap { get; set; }
    public int Distance { get; set; }

    public Elevator(int position, int id)
    {

        this.position = 1;
        this.id = id;
        Movement = "idle";
        Requests = new List<int>();
        Obstacles = false;
        OpenDoors = true;
        Current_position = position;
    }

    public void MoveDown()
    {
        Current_position -= 1;
        Console.WriteLine(this.id + " is currently at floor " + Current_position);
    }


    public void MoveUp()
    {
        Current_position += 1;
        Console.WriteLine(this.id + "  is currently at floor " + Current_position);
    }

    public void CloseDoors()
    {
        while (OpenDoors)
        {
            if (Obstacles)
            {
                OpenDoors = true;
            }
            else
            {
                Thread.Sleep(3000);
                OpenDoors = false;
                Console.WriteLine("The doors close.");
            }
        }
    }

    public void GroundFloor()
    {
        while (Current_position != 7)
        {
            CloseDoors();
            if (Current_position > 7)
            {
                MoveDown();
            }
            else if (Current_position < 7)
            {
                MoveUp();
            }
        }
        OpenDoors = true;
    }



    public void RequestFloor(int floorRequest)
    {

        foreach (int item in Requests)
        {
            if (item == 7)
            {
                CloseDoors();
                GroundFloor();
            }
            else
            {
                if (Current_position > item)
                {
                    CloseDoors();
                    for (var floor = Current_position; floor > item; floor--)
                    {
                        MoveDown();
                        Movement = "down";
                    }
                }
                else if (Current_position < item)
                {
                    CloseDoors();
                    for (var floor = Current_position; floor < item; floor++)
                    {
                        MoveUp();
                        Movement = "up";
                    }

                }
                else
                {
                    Requests.Remove(item);
                    Console.WriteLine(Requests.Count);
                }
                Movement = "idle";
                OpenDoors = true;
                Console.WriteLine("& the doors open.");
            }
        }
    }
}

public class Program
{

    static void Main(string[] args)
    {


        Battery batt = new Battery(true);

        Console.WriteLine("In case of scenario 1: ");
        Console.WriteLine("On the second Column: ");
        batt.ColumnList[1].ListElevators[0].Current_position = 26;
        Console.WriteLine("Elevator 1 is at floor " + batt.ColumnList[1].ListElevators[0].Current_position);
        batt.ColumnList[1].ListElevators[0].Movement = "down";
        Console.WriteLine("Elevator 1 is going " + batt.ColumnList[1].ListElevators[0].Movement);
        batt.ColumnList[1].ListElevators[2].Requests.Add(11);

        batt.ColumnList[1].ListElevators[1].Current_position = 9;
        Console.WriteLine("Elevator 2 is at floor " + batt.ColumnList[1].ListElevators[1].Current_position);
        batt.ColumnList[1].ListElevators[1].Movement = "up";
        Console.WriteLine("Elevator 2 is going " + batt.ColumnList[1].ListElevators[1].Movement);
        batt.ColumnList[1].ListElevators[2].Requests.Add(21);

        batt.ColumnList[1].ListElevators[2].Current_position = 19;
        Console.WriteLine("Elevator 3 is at floor " + batt.ColumnList[1].ListElevators[2].Current_position);
        batt.ColumnList[1].ListElevators[2].Movement = "down";
        Console.WriteLine("Elevator 3 is going " + batt.ColumnList[1].ListElevators[2].Movement);
        batt.ColumnList[1].ListElevators[2].Requests.Add(7);

        batt.ColumnList[1].ListElevators[3].Current_position = 21;
        Console.WriteLine("Elevator 4 is at floor " + batt.ColumnList[1].ListElevators[3].Current_position);
        batt.ColumnList[1].ListElevators[3].Movement = "down";
        Console.WriteLine("Elevator 4 is going " + batt.ColumnList[1].ListElevators[3].Movement);
        batt.ColumnList[1].ListElevators[2].Requests.Add(8);

        batt.ColumnList[1].ListElevators[4].Current_position = 12;
        Console.WriteLine("Elevator 5 is at floor " + batt.ColumnList[1].ListElevators[4].Current_position);
        batt.ColumnList[1].ListElevators[4].Movement = "down";
        Console.WriteLine("Elevator 5 is going " + batt.ColumnList[1].ListElevators[4].Movement);
        batt.ColumnList[1].ListElevators[2].Requests.Add(7);

        batt.ColumnList[1].RequestElevator(7, "up", 26);


        Battery batt2 = new Battery(true);

        Console.WriteLine("In case of scenario 2: ");
        Console.WriteLine("in third column: ");

        batt2.ColumnList[2].ListElevators[0].Current_position = 7;
        Console.WriteLine("Elevator 1 is at floor " + batt2.ColumnList[2].ListElevators[0].Current_position);
        batt2.ColumnList[2].ListElevators[0].Movement = "up";
        Console.WriteLine("Elevator 1 is going " + batt2.ColumnList[2].ListElevators[0].Movement);

        batt2.ColumnList[2].ListElevators[1].Current_position = 29;
        Console.WriteLine("Elevator 2 is at floor " + batt2.ColumnList[2].ListElevators[1].Current_position);
        batt2.ColumnList[2].ListElevators[1].Movement = "up";
        Console.WriteLine("Elevator 2 is going " + batt2.ColumnList[2].ListElevators[1].Movement);

        batt2.ColumnList[2].ListElevators[2].Current_position = 39;
        Console.WriteLine("Elevator 3 is at floor " + batt2.ColumnList[2].ListElevators[2].Current_position);
        batt2.ColumnList[2].ListElevators[2].Movement = "down";
        Console.WriteLine("Elevator 3 is at floor " + batt2.ColumnList[2].ListElevators[2].Movement);

        batt2.ColumnList[2].ListElevators[3].Current_position = 46;
        Console.WriteLine("Elevator 4 is at floor " + batt2.ColumnList[2].ListElevators[3].Current_position);
        batt2.ColumnList[2].ListElevators[3].Movement = "down";
        Console.WriteLine("Elevator 4 is at floor " + batt2.ColumnList[2].ListElevators[3].Movement);

        batt2.ColumnList[2].ListElevators[4].Current_position = 45;
        Console.WriteLine("Elevator 5 is at floor " + batt2.ColumnList[2].ListElevators[4].Current_position);
        batt2.ColumnList[2].ListElevators[4].Movement = "down";
        Console.WriteLine("Elevator 5 is at floor " + batt2.ColumnList[2].ListElevators[4].Movement);

        batt2.ColumnList[2].RequestElevator(7, "up", 42);

    }


}
}


