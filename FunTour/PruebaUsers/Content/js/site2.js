function editFeedback(id) {
    var description = $("#feed-" + id + " #feed-description").val();
    var note = $("#feed-" + id + " #feed-note").val();
    var DataServiceURL = "/feedbacks/Edit/" + id;
    $.ajax({
        type: "POST",
        url: "http://localhost:60350/feedbacks/Edit/?id=" + id + "&description=" + description + "&note=" + note,
        data: "",
        //data: { description: description, id: id, note: note },
        //data: { Description: description, ID: id, Note: note },
        //data: "Description="+description+"&ID="+id+"&Note="+note ,
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    });

    $("#feed-" + id + " #feed-description").toggle();
    $("#feed-" + id + " #feed-note").toggle();
    $("#feed-" + id + " #feed-save").toggle();

    //Estos serian los labels
    $("#feed-" + id + " #feed-note-l").toggle();
    $("#feed-" + id + " #feed-description-l").toggle();
    $("#feed-" + id + " #feed-edit").toggle();
    
    location.reload();
}