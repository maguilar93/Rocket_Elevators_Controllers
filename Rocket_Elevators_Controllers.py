import threading
import time
import operator

column = 1
nbFloors = 10
battery = True
listElevators = []
gapList = []
movList = []
posList = []

class Elevator:
    def __init__(self, position, id):
        self.position = position
        self.id = id
        self.movement = "idle"
        self.requests = []
        self.obstacles = False
        self.openDoors = True
        self.current_position = position

    # moveDown function

    def moveDown(self):
        self.current_position -= 1

    # moveUp function
    def moveUp(self):
        self.current_position += 1

    # closeDoors
    def closeDoors(self):
        while self.openDoors:
            if self.obstacles:
                self.openDoors
                print(self.openDoors)
            else:
                time.sleep(3)
                self.openDoors = False
            break

    # return elevator to position from beginning
    def backToPosition(self):
        while self.current_position != self.position:
            self.closeDoors()
            if self.current_position > self.position:
                self.moveDown()
            elif self.current_position < self.position:
                self.moveUp()
        self.openDoors = True

    # timer as soon as it's idle
    def wait(self):
        if self.movement ==  "idle":
            time.sleep(600)
            self.backToPosition()

    # elevatorButtons
    def requestFloor(self, requestedFloor):
        self.requests.append(requestedFloor)
        self.requests.sort()
        
        while self.current_position != requestedFloor:
            if self.current_position > requestedFloor:
                self.moveDown()
                self.movement = "down"
                self.openDoors()
            elif self.current_position < requestedFloor:
                self.moveUp()
                self.movement = "up"
                self.openDoors()
        self.movement = "idle"
        self.requests.remove(requestedFloor)

    # close button
    def closeButton(self):
        if self.openDoors:
            self.closeDoors()

    # phone button
    def phoneButton(self):
        print("dialing 911...")



# when we request an elevator at any floor
def sendElev(elevator, requestedFloor, direction):
    elevator.requests.append(requestedFloor)
    elevator.requests.sort()

    while elevator.current_position != requestedFloor:
        elevator.closeDoors()
        if elevator.current_position > requestedFloor:
            elevator.moveDown()
        elif elevator.current_position < requestedFloor:
            elevator.moveUp()
    elevator.openDoors = True
    elevator.movement = "idle"

# make a list for positions
def getPositions():
    for k in listElevators:
        posList.append(k.current_position)

# make a list for elevator movements
def getMovements():
    for j in listElevators:
        movList.append(j.movement)

# make a list for gaps
def getGaps(requestedFloor):
    for i in listElevators:
        gapList.append(abs(requestedFloor - i.current_position))


def requestElevator(requestedFloor, direction):

    getPositions()
    getMovements()
    getGaps(requestedFloor)

    if gapList[0] > gapList[1]:
        if movList[1] == "idle":
            sendElev(elevator_2, requestedFloor, direction)
            print("Elevator 2 is idle, so I'm using elevator 2.")
        elif movList[1] == "up" and requestedFloor > posList[1]:
            sendElev(elevator_2, requestedFloor, direction)
            print("Elevator 2 is going up and the requested floor is upstairs, so I'm using elevator 2.")
        elif movList[1] == "up" and requestedFloor < posList[1]: 
            sendElev(elevator_1, requestedFloor, direction) 
            print("Elevator 2 is going up and the request floor is downstairs, so I'm using elevator 1.")
        elif movList[1] == "down" and requestedFloor > posList[1]:
            sendElev(elevator_1, requestedFloor, direction) 
            print("Elevator 2 is going down, and the requested floor is upstairs, so I'm using elevator 1.")
        else: 
            sendElev(elevator_2, requestedFloor, direction) 
            print("Elevator 2 is going down and the requested floor is downstairs, so I'm using elevator 2.")
    else:
        if movList[0] == "idle":
            sendElev(elevator_1, requestedFloor, direction) 
            print("Elevator 1 is idle, so I will use elevator 1.")
        elif movList[0] == "up" and requestedFloor > posList[0]:
            sendElev(elevator_1, requestedFloor, direction) 
            print("Elevator 1 is going up and the requested floor is upstairs so I'm using elevator 1.")
        elif movList[0] == "up" and requestedFloor < posList[0]:
            sendElev(elevator_2, requestedFloor, direction) 
            print("Elevator 1 is going up and the requested floor is downstairs, so I'm using elevator 2.")
        elif movList[0] == "down" and requestedFloor > posList[0]:
            sendElev(elevator_2, requestedFloor, direction) 
            print("Elevator 1 is going down and the requested floor is upstairs, so I'm using elevator 2.")
        else:   
            sendElev(elevator_1, requestedFloor, direction) 
            print("Elevator 1 is going down and the requested floor is downstairs, so I'm using elevator 1.")



elevator_1 = Elevator(1, 1)
elevator_2 = Elevator(5, 2)
elevator_1.movement = "idle"
elevator_2.movement = "idle"
elevator_1.current_position = 2
elevator_2.current_position = 6
listElevators.append(elevator_1)
listElevators.append(elevator_2)

requestElevator(10 , "down")