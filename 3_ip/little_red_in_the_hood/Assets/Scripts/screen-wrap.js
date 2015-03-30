/*
wrap around the screen asteroid style
*/

//leave it here
#pragma strict

//real margin beyond the viewport (1 is the size of the screen, see below)
var overflow = .05;
var yOverflowOverride = 8;


//update is called every frame at fixed intervals
function FixedUpdate()
{
//convert the ship's world position to viewport position
//viewport coordinates are relative to the camera and range from 0 to 1 for everything that is on screen
//x = 0 is the coordinate of the left edge of the screen.
//x = 1 is the coordinate of the right edge of the screen.
var cam = Camera.main;
var viewportPosition = cam.WorldToViewportPoint(transform.position);

//new position is the current position in case nothing changes
var newPosition = viewportPosition;

if (viewportPosition.x > 1 + overflow)
	newPosition.x = 0 - overflow;

if (viewportPosition.x < 0 - overflow)
	newPosition.x = 1 + overflow;

if ((viewportPosition.y > 1 + overflow) || (transform.position.y > yOverflowOverride))
	newPosition.y = 0 - overflow;

if (viewportPosition.y < 0 - overflow) {
	newPosition.y = 1 + overflow;
	transform.position = cam.ViewportToWorldPoint(newPosition);
	transform.position.y = 8;
}
else
	transform.position = cam.ViewportToWorldPoint(newPosition);
}
