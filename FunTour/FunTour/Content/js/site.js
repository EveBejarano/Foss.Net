function toggleEdit(id) {
    //Estos son los inputs con el css "display_none"
    $("#feed-" + id + " #feed-description").toggle();
    $("#feed-" + id + " #feed-note").toggle();
    $("#feed-" + id + " #feed-save").toggle();

    //Estos serian los labels
    $("#feed-" + id + " #feed-note-l").toggle();
    $("#feed-" + id + " #feed-description-l").toggle();
    $("#feed-" + id + " #feed-edit").toggle();
}


