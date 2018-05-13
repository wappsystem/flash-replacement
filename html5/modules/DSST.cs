/////Input Parameters//////
var testtime:Number;
var examplequestions:Number;
var armseq:String;
var vid:String;
var pid:String;
var uid:String;
var rid:String;
var RandomisationNumber:String;
var Initials:String;
var nexttestname:String;
///////////////////////////

/////Output Parameters/////
var testcount:Number=0;
var correctcount:Number=0;
///////////////////////////
var outputServer:String;
var NUM_SYMBOLS:Number = 10;
//var EXAMPLE_QUESTIONS:Number = 5;
//var TEST_TIME:Number = 90000; //milliseconds
var keysActive:Boolean = false;
var myRandGenerator:RandomGenerator = new RandomGenerator(NUM_SYMBOLS);

var testIndex:Number = 0;

var startTime:Number;
var intervalID_Test:Number;

//contains numbers 0 -> NUM_SYMBOLS randomly sorted
var symbolArr:Array = myRandGenerator.generateRandomArray();
randSymHolder_mc._visible=false;
example_txt._visible=false;
loadInput();
//startTest();
start_btn.onPress = function()
{
	start_btn._visible = false;
	randSymHolder_mc._visible = true;
	example_txt._visible=true;
	startTest();
}

function loadInput()
{
	outputServer = "record.add.flash_return.aspx";
	if (_level0.vid == undefined)
	{
		vid = '';
		pid = '';
		rid= '';
		armseq = '';
		nexttestname='';
		testtime = 5000;
		examplequestions = 2;
	}
	else
	{
		vid = _level0.vid;
		pid = _level0.pid;
		rid = _level0.rid;
		armseq = _level0.armseq;
		uid = _level0.uid;
		Initials=_level0.Initials;
		RandomisationNumber=_level0.RandomisationNumber;
		nexttestname=_level0.nexttestname;
		//testFinished_txt.text=vid;
		testtime = Number(_level0.testtime);
		examplequestions = Number(_level0.examplequestions);
	}
}




function startTest()
{		//initialise
		displayRandom();
		keysActive = true;
}

//display random symbol
function displayRandom():Void
{
	testIndex++;
	//If X tests have passed, start the real test
	if (testIndex == examplequestions +1)
	{
		example_txt._visible = false;
		instructions_txt._visible = false;
		startTime = getTimer();
		intervalID_Test = setInterval(Timer, 100);
	}
	else if (testIndex <= examplequestions)
	{
		example_txt.text = "EXAMPLE "+testIndex.toString()+"/"+examplequestions.toString();
	}
	
	var randNum:Number = myRandGenerator.generateRandomNumber();
	randSymHolder_mc.gotoAndStop(symbolArr[randNum]+1);
	keysActive = true;
}

//Handle key-presses
var KeyListener:Object = new Object();
KeyListener.onKeyUp = function()
{
	if (!keysActive) return;
	keysActive = false;
		
	if(testIndex > examplequestions)
	{
		testcount++;
		
		//correct value pressed
		if (Number(chr(Key.getAscii())) == myRandGenerator.latestRandomNumber)
		{
			correctcount++;
		}
		trace(testIndex+": "+correctcount+"/"+testcount);
	}
	displayRandom();
}
Key.addListener(KeyListener);

/* Function: Timer */
function Timer() :Void
{
	timer_txt.text = Math.floor((getTimer() - startTime)/1000);
	if ((getTimer()-startTime) >=testtime) //TEST_TIME)
	{
		keysActive = false;
		clearInterval(intervalID_Test);
		randSymHolder_mc._visible = false;
		finished_txt._visible = true;
		uploadResults();
	
		}
}

function uploadResults()
{
	getURL(outputServer+'?correctcount='+ correctcount.toString()+'&testcount='+testcount.toString()+'&uid='+uid+'&rid='+rid+'&armseq='+armseq+'&pid='+pid+'&vid='+vid+'&nexttestname='+nexttestname+'&RandomisationNumber='+RandomisationNumber+'&Initials='+Initials);
}
