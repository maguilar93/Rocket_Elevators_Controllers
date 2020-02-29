package main

type Elevator struct {
	position         int
	id               int
	movement         string
	requests         []int
	obstacles        bool
	openDoors        bool
	current_position int
	gap              int
	distance         int
	lastFloor        int
}

func elevConstructor(id int) *Elevator {
	a_Elevator := Elevator{}
movement:
	"idle"
obstacles:
	false
openDoors:
	true

	return a_Elevator
}
