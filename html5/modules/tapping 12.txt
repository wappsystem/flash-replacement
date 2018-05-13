import flash.external.ExternalInterface;
//2000-02-05
// Tapping test, match a specified sequence of numbers
//
//
// INPUT
var displaynumbers:Array;
var repeats:Number;
var predelaytime:Number;
var predelaytext:String;
var postdelaytime:Number;
var postdelaytext:String;
var timelimit:Number;
var armseq:String;
var vid:String;
var pid:String;
var uid:String;
var rid:String;
var RandomisationNumber:String;
var Initials:String;
var nexttestname:String;
// END INPUT

// OUTPUT Parameters
var testreactiontime:Array = new Array();
var testsuccess:Array = new Array();
// END OUTPUT

// VARIABLES
var outputServer:String;
var NumberAsString:String = "";
var lastColor:Number = -1;
var seqnumber:Number;
var predelaytimeID:Number;
var postdelaytimeID:Number;
var timelimitID:Number;
var startTime:Number;
var story:XML = new XML();
var KeyListener:Object = new Object();
var numbers:Array;
var testCount:Number=0;
var	trialresults:Array = new Array();
var	trialtimes:Array = new Array();
var	trialcount:Array = new Array();

// END VARIABLES
  
/* INIT APP */
testInfo_instruction1._visible = true;
testInfo_instruction2._visible = true;
saveMessage_txt._visible = false;
serverError_txt._visible = false;
testInfo_txt._visible = false;
clearMarker();
loadInput();

/* END INIT */

start_btn.onPress = function()
{
	start_btn._visible = false;
	Instruction_text._visible =false;
	showpreinfo();
}
function loadInput()
{
		vid = '0';
		pid = '0';
		armseq = '0';
		nexttestname='0';
		repeats=12;
		predelaytime = 5000;
		predelaytext = "Be ready it will start shortly";
		postdelaytime = 25000;
		postdelaytext = "Relax for a while.";
		timelimit = 30000;
		makeNumbersInvisible()
		var NumberAsString:String  = "4,1,3,2,4";
		numbers = NumberAsString.split(",");
		Number1.text =numbers[0];
		Number2.text =numbers[1];
		Number3.text =numbers[2];
		Number4.text =numbers[3];
		Number5.text =numbers[4];
		
		content_txt.multiline= true;
		content_txt.wordWrap = true;
		content_txt.html = true;
		/*var story:XML = new XML();
		story.ignoreWhite = true;
		story.load("instruction_col.htm");
		story.onLoad = function () {	
		content_txt.htmlText = story;
		}
		*/
}



function showpreinfo()
{
	predelaytimeID = setInterval(predelaytimeReached,predelaytime);
	content_txt._visible =false;
	testInfo_instruction1._visible = false;
	testInfo_instruction2._visible = false;
	testInfo_txt.text = predelaytext;
	testInfo_txt._visible = true;

}
function showpostinfo()
{
	repeats=repeats-1;
	if (repeats > 0)
	{
	postdelaytimeID = setInterval(postdelaytimeReached,postdelaytime);
	clearMarker();
	makeNumbersInvisible();
	testInfo_txt.text = postdelaytext;
	testInfo_txt._visible = true;
	}
	else {
		uploadResults();
	}


}
function startApp()
{
	makeNumbersVisible();
	timelimitID = setInterval(timelimitReached,timelimit);
	seqnumber=0;
	testCount++;
	startResponseTime=getTimer();
	Key.addListener(KeyListener);


	//userClicked takes over from here

	//timelimitReached will end the app
}
KeyListener.onKeyUp = function()
{
	Key.removeListener(KeyListener);
	responded = true;
	var responseTime:Number = getTimer() - startResponseTime;
	var match:Boolean;
	var success:Boolean;
	var wrongKey:Boolean = false;
	var resultString:String;
	//match key pressed
	if ((chr(Key.getAscii())) == numbers[seqnumber]) resultString = "M";
	else resultString = "N";
	
	var resultsArr:Array = new Array(resultString,responseTime,testCount);	
	writeData(resultsArr);
	showmarker(seqnumber);
	seqnumber=seqnumber+1;
	if (seqnumber > 4) {
		seqnumber=0;
	}
	startResponseTime=getTimer();
	Key.addListener(KeyListener);
}
function showmarker(markernumber:Number)
{
	if (markernumber==0) Marker1._visible =true; 
	if (markernumber==1) Marker2._visible =true; 
	if (markernumber==2) Marker3._visible =true; 
	if (markernumber==3) Marker4._visible =true; 
	if (markernumber==4) 
	{
		Marker5._visible =true;
		ClearMarkerID = setInterval(clearMarker,50);
	}
}
function clearMarker()
{
	clearInterval(ClearMarkerID);
		Marker1._visible =false; 
		Marker2._visible =false;
		Marker3._visible =false;
		Marker4._visible =false;
		Marker5._visible =false;
}

function writeData(dataArr:Array):Void
{
	var resultStr:String = "";
	for (var i:Number = 0; i < dataArr.length; i++)
	{
		resultStr += dataArr[i] + " ";
	}
	trace(resultStr);
	trialresults.push(dataArr[0]);
	trialtimes.push(dataArr[1]);
	trialcount.push(dataArr[2]);
}
function predelaytimeReached()
{
	clearInterval(predelaytimeID);
	testInfo_txt._visible = false;
	startApp();
}

function postdelaytimeReached()
{
	clearInterval(postdelaytimeID);
	testInfo_txt._visible = false;
	showpreinfo()

}

function timelimitReached()
{
	clearInterval(timelimitID);
	makeNumbersInvisible();
	Key.removeListener(KeyListener);
	showpostinfo();

}

function uploadResults()
{	
	//ver 3 call javascript
	testFinished_txt.text='Upload Results';
	var param=	
			'&Tapping_Time='+trialtimes.toString()+
			'&Match='+trialresults.toString()+
			'&Sequence_Number='+trialcount.toString();
	ExternalInterface.call("g_tapping_12_callback",param);
	testFinished_txt._visible = false;
}

function makeNumbersVisible()
{
				Number1._visible=true;
				Number2._visible=true;
				Number3._visible=true;
				Number4._visible=true;
				Number5._visible=true;
	}
function makeNumbersInvisible()
{				Number1._visible=false;
				Number2._visible=false;
				Number3._visible=false;
				Number4._visible=false;
				Number5._visible=false;
}

