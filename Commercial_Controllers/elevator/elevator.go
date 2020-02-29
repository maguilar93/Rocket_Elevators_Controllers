package elevator

type Elevator struct {
	Position         int
	Id               int
	Movememnt        string
	Requests         []int
	Obstacles        bool
	OpenDoors        bool
	Current_position int
	Gap              int
	Distance         int
	LastFloor        int
}
