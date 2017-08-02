
+function ($) {

    var Items = [];

    function addNewTask(parent) {

        updateTask(parent, 0);

    }

    function deleteTask(parent, taskId) {
        var answer = confirm("Are you shure?");
        if (!answer) {
            return;
        }

        $.ajax({
            type: "DELETE",
            url: "../api/tasks/" + taskId,
            dataType: "json",
            success: function (data) {
                Items = data;
                redrawItems(parent, data);
                //updateItems(parent);
            }
        });

    }

    function updateTask(parent, taskId) {
        var modal = $("<div></div>").addClass("tasks-modal-background"); //.height($(window).height());
        modal.on("click", function () {
            $(".task-editor").remove("*");
            $(".tasks-modal-background").remove("*");
        });

        var item = {
            TaskId: 0,
            Title: "",
            Content: "",
            LastUpdate: ""
        };

        if (taskId > 0) {
            for (var i in Items) {
                if (Items[i]['TaskId'] == taskId) {
                    item = Items[i];
                    break;
                }
            }
        }

        var editor = $("<div></div>").addClass("task-editor").html("\
            <div>\
            <input type='text' id='update_task_title' value='" + item.Title + "' placeholder='Enter title here'/>\
            </div>\
            <div class='update_task_content_container'>\
            <textarea id='update_task_content'>"+ item.Content + "</textarea>\
            </div>\
            <div class='update_task_save_container'>\
            <button class='tasks-tools-button' id='update_task_save'>Save</button>\
            </div>\
        ");

        $("body").append(modal).append(editor);
        $("#update_task_save").on("click", function () {
            saveTask(parent, taskId);
        });

        $(window).on("resize", function () {
            $(".tasks-modal-background").height($(window).height());
            $("#update_task_content").height($(".task-editor").height() - 153);
        });

        $(window).resize();

    }

    function saveTask(parent, taskId) {

        var task_title = $("#update_task_title").val();
        var task_content = $("#update_task_content").val();

        if (task_title === 'undefined' || task_title == "") {
            alert("Title can not be empty !");
            return;
        }

        var item = {
            TaskId: taskId,
            Title: task_title,
            Content: task_content
        };

        $.ajax({
            type: taskId == 0 ? "POST" : "PUT",
            url: "../api/tasks" + (taskId == 0 ? "" : "/" + taskId),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(item),
            success: function (data) {

                $(".task-editor").remove("*");
                $(".tasks-modal-background").remove("*");

                Items = data;
                redrawItems(parent, data);
                //updateItems(parent);
            }
        });

    }

    function updateItems(parent) {

        $.getJSON("../api/tasks", function (data) {
            // Для простоты каждый раз буду перечитывать все задачи и полностью перерисовывать список.
            // Хотя по-хорошему надо бы отслеживать только измененные записи и перерисовывать только их...

            Items = data;
            redrawItems(parent, data);

        });
    }

    function redrawItems(parent, items) {

        //Items = items;

        var tasksList = $(parent).find(".tasks-list").first();
        tasksList.children().remove("*");

        for (var i in items) {
            createItem(parent, tasksList, items[i]);
        }

    }

    function searchItems(parent, searchString) {

        var items = [];
        var ss = searchString.toLowerCase();

        for (var i in Items) {
            var $item = $(Items[i]);
            if ($item.attr("Title").toLowerCase().indexOf(ss) >= 0 || $item.attr("Content").toLowerCase().indexOf(ss) >= 0)
            {
                items.push(Items[i]);
            }
        }

        redrawItems(parent, items);

    }

    function searchCancel(parent) {

        redrawItems(parent, Items);

    }

    function createItem(parent, list, item) {

        var $list = $(list);

        var taskItem = $("<div></div>").addClass("tasks-list-item").attr("task-id", item.TaskId);
        var itemHeader = $("<div></div>").addClass("task-item-header");
        var itemTitle = $("<div></div>").addClass("task-item-title").html("<h2>" + item.Title + "</h2>");
        itemHeader.append(itemTitle);

        var itemDelete = $("<div></div>").addClass("task-item-delete").on('click', function () {
            deleteTask(parent, item.TaskId);
        });
        itemHeader.append(itemDelete);

        var itemUpdate = $("<div></div>").addClass("task-item-update").on('click', function () {
            updateTask(parent, item.TaskId);
        });
        itemHeader.append(itemUpdate);

        taskItem.append(itemHeader);

        var itemContent = $("<div></div>").addClass("task-item-content").html("<p>" + item.Content + "</p>");
        taskItem.append(itemContent);

        var itemDateTime = $("<div></div>").addClass("task-item-footer").html("<span>" + item.LastUpdate + "</span>");
        taskItem.append(itemDateTime);

        $list.append(taskItem);
    }

    function createContent(parent) {

        var tasksCaption = $("<div></div>").addClass("tasks-caption").html("<h3>All tasks</h3>");
        parent.append(tasksCaption);

        var tasksBody = $("<div></div>").addClass("tasks-body");
        var tasksTools = $("<div></div>").addClass("tasks-tools");
        var tasksList = $("<div></div>").addClass("tasks-list");
        var newTaskButton = $("<button></button>").addClass("tasks-tools-button").text("Add new task").on("click", function () {
            addNewTask(parent);
        });

        var searchElem = $("<div></div>").addClass("tasks-tools-search")
            .html("<input type='text' id='search-field' name='search-field' value='' placeholder='Search'/>");

        tasksTools.append(newTaskButton);
        tasksTools.append(searchElem);
        tasksBody.append(tasksTools);
        tasksBody.append(tasksList);

        parent.append(tasksBody);

        $("#search-field").on('input', function () {
            var val = $(this).val();
            if (val.length >= 3) {
                searchItems(parent, val);
            }
            else {
                searchCancel(parent);
            }
        });

        updateItems(parent);

    }

    $.fn.tasks = function () {
        this.addClass("tasks-content");
        createContent(this);
        return this;
    }

}(window.jQuery);
