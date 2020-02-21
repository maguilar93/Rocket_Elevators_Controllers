# Rocket_Elevators_Controllers

This repository contains:
_ the algorithms for the New Rocket Elevator Solutions for both Residential and Commercial offers
_ the Python version of the algorithm for the Residential offer \* the Javscript version of the algorithm for the Residential offer

## Getting started

### Requirements

- Node.js
- Python

### Instructions

1. In order to use the Python and Javascript versions, it is important to create the two elevators with the main positions desired.

   For Python:

   ```
   elevator_1 = Elevator(position, id)
   elevator_2 = Elevator(position, id)
   ```

   For Javascript:

   ```
   let elevator_1 = new Elevator(position, id);
   let elevator_2 = new Elevator(position, id);
   ```

2. By default, the elevators created will be idle and the position chosen will be the current position. In order to change these parameters, it is important to change them manually:

   For Python:

   ```
   elevator_1.movement = "up"
   elevator_1.current_position = 2
   ```

   For Javascript:

   ```
   elevator_1.movement = "up";
   elevator_1.current_position = 2;
   ```

3. Since the two elevators were just created, they must be inserted into their respective list/array for the program to work.

   For Python:

   ```
   listElevators.append(elevator_1)
   listElevators.append(elevator_2)
   ```

   For Javascript:

   ```
   listElevators.push(elevator_1);
   listElevators.push(elevator_2);
   ```

4. If someone wishes to request an elevator, the requestElevator function must be called with the appropriate parameters.

   For Python:

   ```
   requestElevator(requestedFloor, direction)
   ```

   For Javascript:

   ```
   requestElevator(requestedFloor, direction)
   ```

5. Once the elevator has arrived and the person is inside, the elevator will execute the request by calling the requestFloor with the appropriate parameters, based on the elevator that arrives.

   For Python:

   ```
   elevator_1.requestFloor(requestedFloor)
   elevator_2.requestFloor(requestedFloor)
   ```

   For Javascript:

   ```
   elevator_1.requestFloor(requestedFloor)
   elevator_2.requestFloor(requestedFloor)
   ```
