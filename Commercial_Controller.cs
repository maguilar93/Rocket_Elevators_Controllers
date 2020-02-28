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
            this.ColumnList.Add(new Column(27, 46, 3));
            this.ColumnList.Add(new Column(47, 66, 4));

        }

        public bool RequestColumn(int requestedFloor, string direction, int destination)
        {


            foreach (Column col in ColumnList)
            {

                if (requestedFloor >= col.startFloor && requestedFloor <= col.endFloor || destination >= col.startFloor && destination <= col.endFloor)
                {
                    col.RequestElevator(requestedFloor, direction, destination);
                    Console.WriteLine("In this case, we will use the column #" + col.id);

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

        public void RemoveRequest(int requestedFloor, int destination)
        {
            foreach (Elevator elev in ListElevators)
            {
                if (elev.Current_position == requestedFloor || elev.Current_position == destination)
                {
                    elev.Requests.Remove(requestedFloor);
                    elev.Requests.Remove(destination);
                }
            }
        }


        public bool RequestElevator(int requestedFloor, string direction, int destination)
        {


            foreach (Elevator elev in ListElevators)
            {
                elev.Gap = Math.Abs(requestedFloor - elev.Current_position);
                elev.Distance = Math.Abs(elev.LastFloor + elev.Gap);

                if (elev.Requests.Count() < 1)
                {
                    elev.Requests.Add(0);
                }

                elev.LastFloor = elev.Requests[elev.Requests.Count - 1];
                elev.Distance = Math.Abs(elev.LastFloor + elev.Gap);

            }



            IEnumerable<Elevator> query = ListElevators.OrderBy(elev => elev.Distance);
            var distanceList = query.ToList();


            IEnumerable<Elevator> query2 = ListElevators.OrderBy(elev => elev.Gap);
            var orderedList = query2.ToList();


            foreach (Elevator cage in ListElevators)
            {
                cage.Requests.Add(requestedFloor);
                cage.Requests.Add(destination);

                if ((cage.Current_position == requestedFloor && cage.Movement == direction) || (cage.Movement == "idle") || (cage.Movement == "up" && direction == "up" && cage.Current_position < requestedFloor) || (cage.Movement == "down" && direction == "down" && cage.Current_position > requestedFloor))
                {
                    var curCage = orderedList[0];

                    Console.WriteLine("You have arrived to your destination with elevator # " + curCage.id);

                    return true;

                }
                else
                {

                    cage.Requests.Add(requestedFloor);
                    cage.Requests.Add(destination);
                    var curCage2 = distanceList[0];

                    curCage2.RequestFloor(destination);
                    Console.WriteLine("You have arrived to your destination with elevator #  " + curCage2.id);

                    return true;
                }
            }
            RemoveRequest(requestedFloor, destination);
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
        public int LastFloor { get; set; }

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
            //Console.WriteLine(this.id + " is currently at floor " + Current_position);
        }


        public void MoveUp()
        {
            Current_position += 1;
            //Console.WriteLine(this.id + "  is currently at floor " + Current_position);
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
                    Console.WriteLine("the doors close.");
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



        public void RequestFloor(int destination)
        {
            Requests.Add(destination);

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
        static void Scenario1()
        {
            Battery batt = new Battery(true);

            Console.WriteLine("On the second Column: ");
            batt.ColumnList[1].ListElevators[0].Current_position = 26;
            Console.WriteLine("Elevator 1 is at floor " + batt.ColumnList[1].ListElevators[0].Current_position);
            batt.ColumnList[1].ListElevators[0].Movement = "down";
            Console.WriteLine("Elevator 1 is going " + batt.ColumnList[1].ListElevators[0].Movement);
            batt.ColumnList[1].ListElevators[0].Requests.Add(11);


            batt.ColumnList[1].ListElevators[1].Current_position = 9;
            Console.WriteLine("Elevator 2 is at floor " + batt.ColumnList[1].ListElevators[1].Current_position);
            batt.ColumnList[1].ListElevators[1].Movement = "up";
            Console.WriteLine("Elevator 2 is going " + batt.ColumnList[1].ListElevators[1].Movement);
            batt.ColumnList[1].ListElevators[1].Requests.Add(21);


            batt.ColumnList[1].ListElevators[2].Current_position = 19;
            Console.WriteLine("Elevator 3 is at floor " + batt.ColumnList[1].ListElevators[2].Current_position);
            batt.ColumnList[1].ListElevators[2].Movement = "down";
            Console.WriteLine("Elevator 3 is going " + batt.ColumnList[1].ListElevators[2].Movement);
            batt.ColumnList[1].ListElevators[2].Requests.Add(7);


            batt.ColumnList[1].ListElevators[3].Current_position = 21;
            Console.WriteLine("Elevator 4 is at floor " + batt.ColumnList[1].ListElevators[3].Current_position);
            batt.ColumnList[1].ListElevators[3].Movement = "down";
            Console.WriteLine("Elevator 4 is going " + batt.ColumnList[1].ListElevators[3].Movement);
            batt.ColumnList[1].ListElevators[3].Requests.Add(8);


            batt.ColumnList[1].ListElevators[4].Current_position = 12;
            Console.WriteLine("Elevator 5 is at floor " + batt.ColumnList[1].ListElevators[4].Current_position);
            batt.ColumnList[1].ListElevators[4].Movement = "down";
            Console.WriteLine("Elevator 5 is going " + batt.ColumnList[1].ListElevators[4].Movement);
            batt.ColumnList[1].ListElevators[4].Requests.Add(7);

            batt.RequestColumn(7, "up", 26);
        }

        static void Scenario2()
        {
            Battery batt2 = new Battery(true);

            Console.WriteLine("In case of scenario 2: ");
            Console.WriteLine("in third column: ");

            batt2.ColumnList[2].ListElevators[0].Current_position = 7;
            Console.WriteLine("Elevator 1 is at floor " + batt2.ColumnList[2].ListElevators[0].Current_position);
            batt2.ColumnList[2].ListElevators[0].Movement = "up";
            Console.WriteLine("Elevator 1 is going " + batt2.ColumnList[2].ListElevators[0].Movement);
            batt2.ColumnList[2].ListElevators[0].Requests.Add(46);


            batt2.ColumnList[2].ListElevators[1].Current_position = 29;
            Console.WriteLine("Elevator 2 is at floor " + batt2.ColumnList[2].ListElevators[1].Current_position);
            batt2.ColumnList[2].ListElevators[1].Movement = "up";
            Console.WriteLine("Elevator 2 is going " + batt2.ColumnList[2].ListElevators[1].Movement);
            batt2.ColumnList[2].ListElevators[1].Requests.Add(34);

            batt2.ColumnList[2].ListElevators[2].Current_position = 39;
            Console.WriteLine("Elevator 3 is at floor " + batt2.ColumnList[2].ListElevators[2].Current_position);
            batt2.ColumnList[2].ListElevators[2].Movement = "down";
            Console.WriteLine("Elevator 3 is going " + batt2.ColumnList[2].ListElevators[2].Movement);
            batt2.ColumnList[2].ListElevators[2].Requests.Add(7);

            batt2.ColumnList[2].ListElevators[3].Current_position = 46;
            Console.WriteLine("Elevator 4 is at floor " + batt2.ColumnList[2].ListElevators[3].Current_position);
            batt2.ColumnList[2].ListElevators[3].Movement = "down";
            Console.WriteLine("Elevator 4 is going " + batt2.ColumnList[2].ListElevators[3].Movement);
            batt2.ColumnList[2].ListElevators[3].Requests.Add(30);

            batt2.ColumnList[2].ListElevators[4].Current_position = 45;
            Console.WriteLine("Elevator 5 is at floor " + batt2.ColumnList[2].ListElevators[4].Current_position);
            batt2.ColumnList[2].ListElevators[4].Movement = "down";
            Console.WriteLine("Elevator 5 is going " + batt2.ColumnList[2].ListElevators[4].Movement);
            batt2.ColumnList[2].ListElevators[4].Requests.Add(7);

            batt2.RequestColumn(7, "up", 42);
        }

        static void Scenario3()
        {
            Battery batt3 = new Battery(true);

            Console.WriteLine("In case of scenario 3: ");
            Console.WriteLine("in fourth column: ");

            batt3.ColumnList[3].ListElevators[0].Current_position = 64;
            Console.WriteLine("Elevator 1 is at floor " + batt3.ColumnList[3].ListElevators[0].Current_position);
            batt3.ColumnList[3].ListElevators[0].Movement = "down";
            Console.WriteLine("Elevator 1 is going " + batt3.ColumnList[3].ListElevators[0].Movement);
            batt3.ColumnList[3].ListElevators[0].Requests.Add(7);


            batt3.ColumnList[3].ListElevators[1].Current_position = 56;
            Console.WriteLine("Elevator 2 is at floor " + batt3.ColumnList[3].ListElevators[1].Current_position);
            batt3.ColumnList[3].ListElevators[1].Movement = "up";
            Console.WriteLine("Elevator 2 is going " + batt3.ColumnList[3].ListElevators[1].Movement);
            batt3.ColumnList[3].ListElevators[1].Requests.Add(66);

            batt3.ColumnList[3].ListElevators[3].Current_position = 52;
            Console.WriteLine("Elevator 3 is at floor " + batt3.ColumnList[3].ListElevators[3].Current_position);
            batt3.ColumnList[3].ListElevators[3].Movement = "up";
            Console.WriteLine("Elevator 3 is at floor " + batt3.ColumnList[3].ListElevators[3].Movement);
            batt3.ColumnList[3].ListElevators[3].Requests.Add(64);

            batt3.ColumnList[3].ListElevators[3].Current_position = 7;
            Console.WriteLine("Elevator 4 is at floor " + batt3.ColumnList[3].ListElevators[3].Current_position);
            batt3.ColumnList[3].ListElevators[3].Movement = "up";
            Console.WriteLine("Elevator 4 is at floor " + batt3.ColumnList[3].ListElevators[3].Movement);
            batt3.ColumnList[3].ListElevators[3].Requests.Add(60);

            batt3.ColumnList[3].ListElevators[4].Current_position = 66;
            Console.WriteLine("Elevator 5 is at floor " + batt3.ColumnList[3].ListElevators[4].Current_position);
            batt3.ColumnList[3].ListElevators[4].Movement = "down";
            Console.WriteLine("Elevator 5 is at floor " + batt3.ColumnList[3].ListElevators[4].Movement);
            batt3.ColumnList[3].ListElevators[4].Requests.Add(7);

            batt3.RequestColumn(60, "down", 7);
        }

        static void Scenario4()
        {
            Battery batt4 = new Battery(true);

            Console.WriteLine("In case of scenario 4: ");
            Console.WriteLine("in first column: ");

            batt4.ColumnList[0].ListElevators[0].Current_position = 3;
            Console.WriteLine("Elevator 1 is at floor " + batt4.ColumnList[0].ListElevators[0].Current_position);
            batt4.ColumnList[0].ListElevators[0].Movement = "idle";
            Console.WriteLine("Elevator 1 is going " + batt4.ColumnList[0].ListElevators[0].Movement);

            batt4.ColumnList[0].ListElevators[1].Current_position = 7;
            Console.WriteLine("Elevator 2 is at floor " + batt4.ColumnList[0].ListElevators[1].Current_position);
            batt4.ColumnList[0].ListElevators[1].Movement = "idle";
            Console.WriteLine("Elevator 2 is going " + batt4.ColumnList[0].ListElevators[1].Movement);

            batt4.ColumnList[0].ListElevators[2].Current_position = 4;
            Console.WriteLine("Elevator 3 is at floor " + batt4.ColumnList[0].ListElevators[2].Current_position);
            batt4.ColumnList[0].ListElevators[2].Movement = "down";
            Console.WriteLine("Elevator 3 is going " + batt4.ColumnList[0].ListElevators[2].Movement);
            batt4.ColumnList[0].ListElevators[2].Requests.Add(2);

            batt4.ColumnList[0].ListElevators[3].Current_position = 1;
            Console.WriteLine("Elevator 4 is at floor " + batt4.ColumnList[0].ListElevators[3].Current_position);
            batt4.ColumnList[0].ListElevators[3].Movement = "up";
            Console.WriteLine("Elevator 4 is going  " + batt4.ColumnList[0].ListElevators[3].Movement);
            batt4.ColumnList[0].ListElevators[3].Requests.Add(7);

            batt4.ColumnList[0].ListElevators[4].Current_position = 6;
            Console.WriteLine("Elevator 5 is at floor " + batt4.ColumnList[0].ListElevators[4].Current_position);
            batt4.ColumnList[0].ListElevators[4].Movement = "down";
            Console.WriteLine("Elevator 5 is going " + batt4.ColumnList[0].ListElevators[4].Movement);
            batt4.ColumnList[0].ListElevators[4].Requests.Add(1);

            batt4.RequestColumn(4, "up", 7);
        }
        static void Main(string[] args)
        {

            //Scenario1();
            //Scenario2();
            //Scenario3();
            Scenario4();



        }


    }
}


