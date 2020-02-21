const column = 1;
const nbFloors = 10;
const battery = true;
let listElevators = [];
let gapList = [];
let movList = [];
let posList = [];

class Elevator {
    constructor(position, id) {
        this.position = position;
        this.id = id;
        this.movement = "idle";
        this.requests = [];
        this.obstacles = false;
        this.openDoors = true;
        this.current_position = position;
    }

    // move down function
    moveDown() {
        this.current_position -= 1
    }

    // move up function
    moveUp() {
        this.current_position += 1
    }

    // close doors
    close() {
        this.openDoors = false;
    }

    closeDoors() {
        while (this.openDoors) {
            if (this.obstacles) {
                this.openDoors
            } else {
                this.openDoors = setTimeout(
                    this.close, 3000)
            }
            break
        }
    }

    // returns the elevator to its original position
    backToPosition() {
        while (this.current_position != this.position) {
            this.closeDoors();
            if (this.current_position > this.position) {
                this.moveDown();
                console.log(this.current_position);
            } else if (this.current_position < this.position) {
                this.moveUp();
            }
            console.log(this.current_position)
        }
        this.openDoors = true

    }

    wait() {
        if (this.movement == "idle") {
            this.current_position = setTimeout(this.backToPosition, 6000)
        }
    }

    // get the elevator to the floor requested when the person is inside the elevator
    requestFloor(requestedFloor) {
        this.requests.push(requestedFloor);
        this.requests.sort();
        console.log(this.requests);

        for (var i = 0; i < this.requests.length; i++) {
            if (this.current_position > this.requests[i]) {
                this.closeDoors();
                for (var floor = this.current_position; floor > this.requests[i]; floor--) {
                    this.moveDown();
                    this.movement = "down";
                    this.openDoors == true;
                    console.log(elevator_1.current_position);
                }
            } else if (this.current_position < this.requests[i]) {
                this.closeDoors();
                for (var floor = this.current_position; floor < this.requests[i]; floor++) {
                    this.moveUp();
                    this.movement = "up";
                    this.openDoors == true;
                    console.log(elevator_1.movement);
                    console.log(elevator_1.current_position)
                }
            } else {
                this.movement = "idle";
                this.requests.pop();
                console.log(elevator_1.movement)
                console.log(elevator_1.openDoors)
            }
        }
    }

    // the close button in case of emergency
    closeButton() {
        if (this.openDoors == true) {
            this.closeDoors()
        }
    }

    // the phone button in case of emergency
    phoneButton() {
        console.log("dialing 911...")
    }
}

// this function is called to send the elevator when it is requested at any floor
function sendElev(elevator, requestedFloor, direction) {
    elevator.requests.push(requestedFloor);
    elevator.requests.sort();

    for (var i = 0; i < elevator.requests.length; i++) {
        elevator.closeDoors();

        if (elevator.current_position > elevator.requests[i]) {
            for (var floor = elevator.current_position; floor > elevator.requests[i]; floor--) {
                elevator.moveDown();
                elevator.movement = direction;
                elevator.openDoors == true;
            }
        } else if (elevator.current_position < elevator.requests[i]) {
            for (var floor = elevator.current_position; floor < elevator.requests[i]; floor++) {
                elevator.moveUp();
                elevator.movement = direction;
                elevator.openDoors == true;
            }
        } else {
            elevator.openDoors = true;
            elevator.movement = "idle";
            elevator.requests.pop();
        }
    }
}

// make a list for positions
function getPositions() {
    for (var k = 0; k < listElevators.length; k++) {
        posList.push(listElevators[k].current_position)
    };
}

// make a list for elevator movements 
function getMovements() {
    for (var j = 0; j < listElevators.length; j++) {
        movList.push(listElevators[j].movement);
    };
}

// make a list for gaps
function getGaps(requestedFloor) {
    for (var i = 0; i < listElevators.length; i++) {
        gapList.push(Math.abs(requestedFloor - listElevators[i].current_position))
    };
}

// this function allows you to request an elevator from any floor.
function requestElevator(requestedFloor, direction) {

    getPositions();
    getMovements();
    getGaps(requestedFloor);

    if (gapList[0] > gapList[1]) {
        if (movList[1] == "idle") {
            sendElev(elevator_2, requestedFloor, direction)
            console.log("Elevator 2 is idle, so I'm using elevator 2.")
        } else if (movList[1] == "up" && requestedFloor > posList[1]) {
            sendElev(elevator_2, requestedFloor, direction)
            console.log("Elevator 2 is going up and the requested floor is upstairs, so I'm using elevator 2.")
        } else if (movList[1] == "up" && requestedFloor < posList[1]) {
            sendElev(elevator_1, requestedFloor, direction)
            console.log("Elevator 2 is going up and the request floor is downstairs, so I'm using elevator 1.")
        } else if (movList[1] == "down" && requestedFloor > posList[1]) {
            sendElev(elevator_1, requestedFloor, direction)
            console.log("Elevator 2 is going down, and the requested floor is upstairs, so I'm using elevator 1.")
        } else {
            sendElev(elevator_2, requestedFloor, direction)
            console.log("Elevator 2 is going down and the requested floor is downstairs, so I'm using elevator 2.")
        }
    } else {
        if (movList[0] == "idle") {
            sendElev(elevator_1, requestedFloor, direction)
            console.log("Elevator 1 is idle, so I will use elevator 1.")
        } else if (movList[0] == "up" && requestedFloor > posList[0]) {
            sendElev(elevator_1, requestedFloor, direction)
            console.log("Elevator 1 is going up and the requested floor is upstairs so I'm using elevator 1.")
        } else if (movList[0] == "up" && requestedFloor < posList[0]) {
            sendElev(elevator_2, requestedFloor, direction)
            console.log("Elevator 1 is going up and the requested floor is downstairs, so I'm using elevator 2.")
        } else if (movList[0] == "down" && requestedFloor > posList[0]) {
            sendElev(elevator_2, requestedFloor, direction)
            console.log("Elevator 1 is going down and the requested floor is upstairs, so I'm using elevator 2.")
        } else {
            sendElev(elevator_1, requestedFloor, direction)
            console.log("Elevator 1 is going down and the requested floor is downstairs, so I'm using elevator 1.")
        }
    }
}


let elevator_1 = new Elevator(1, 1);
let elevator_2 = new Elevator(5, 2);
elevator_1.movement = "idle";
elevator_2.movement = "idle";
elevator_1.current_position = 2;
elevator_2.current_position = 6;
listElevators.push(elevator_1);
listElevators.push(elevator_2);


requestElevator(10, "down");