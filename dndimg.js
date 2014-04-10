$(document).ready(function() {
	var dropbox = document.getElementById("dropbox");

	// init event handlers
	if (dropbox.addEventListener)
	{
		dropbox.addEventListener("dragenter", dragEnter, false);
		dropbox.addEventListener("dragexit", dragExit, false);
		dropbox.addEventListener("dragover", dragOver, false);
		dropbox.addEventListener("drop", drop, false);
	}
	else
	{
		//for ie8
		dropbox.attachEvent("ondragenter", dragEnter);
		dropbox.attachEvent("ondragexit", dragExit);
		dropbox.attachEvent("ondragover", dragOver);
		dropbox.attachEvent("ondrop", drop);
	}
	// init the widgets
	//$("#progressbar").progressbar();
});

function dragEnter(evt) {
	stopEvent(evt);
}

function dragExit(evt) {
	stopEvent(evt);
}

function dragOver(evt) {
	stopEvent(evt);
}

function drop(evt) {
	stopEvent(evt);

	if (evt.dataTransfer && evt.dataTransfer.files)
	{
		var files = evt.dataTransfer.files;
		var count = files.length;

		// Only call the handler if 1 or more files was dropped.
		if (count > 0)
			handleFiles(files);
	}
	else
	{
		$("#droplabel").text("Drag & Drop of files is not supported."); // IE8
	}
}

function stopEvent(e) {
 
	if(!e) var e = window.event;
 
	//e.cancelBubble is supported by IE8 -
        // this will kill the bubbling process.
	e.cancelBubble = true;
	e.returnValue = false;
 
	//e.stopPropagation works only in Firefox.
	if ( e.stopPropagation ) e.stopPropagation();
	if ( e.preventDefault ) e.preventDefault();		
 
       return false;
}

function handleFiles(files) {
	var file = files[0];

	$("#droplabel").text("Processing " + file.name);

	var reader = new FileReader();

	// init the reader event handlers
	$("#droplabel").text("Loading...");
	reader.onprogress = handleReaderProgress;
	reader.onloadend = handleReaderLoadEnd;

	// begin the read operation
	reader.readAsDataURL(file);
}

function handleReaderProgress(evt) {
	if (evt.lengthComputable) 
	{
		var loaded = (evt.loaded / evt.total);

	 	$("#droplabel").text($("#droplabel").text() + ".");

		//$("#progressbar").progressbar({ value: loaded * 100 });
	}
}

function handleReaderLoadEnd(evt) {
	//$("#progressbar").progressbar({ value: 100 });

	//var img = document.getElementById("preview");
	//img.src = evt.target.result;

	//send image to server
	send(evt.target.result);

	$("#droplabel").text("Drop file here...");

}