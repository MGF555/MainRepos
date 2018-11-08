var url = "http://localhost:62928";

function fail(response) {
    console.log("FAILED!", response);
}

function getLastResult() {
    if (vue.searchCategory && vue.searchTerm) {
        search(vue.searchCategory, vue.searchTerm);
    } else {
        getAll();
    }
}

function getAll() {
    $.get(url + "/dvds")
        .done(function (response) {
            vue.dvds = response;
        }).fail(fail);
}

function post(dvd) {
    $.ajax({
        type: "POST",
        url: url + "/dvd",
        data: JSON.stringify(dvd),
        contentType: "application/json"
    }).done(function (response) {
        getLastResult();
    }).fail(fail);
}

function put(dvd) {
    $.ajax({
        type: "PUT",
        url: url + "/dvd/" + dvd.DvdId,
        data: JSON.stringify(dvd),
        contentType: "application/json"
    }).done(function (response) {
        getLastResult();
    }).fail(fail);
}

function remove(dvd) {
    $.ajax({
        type: "DELETE",
        url: url + "/dvd/" + dvd.DvdId
    }).done(function (response) {
        getLastResult();
    }).fail(fail);
}

function search(searchCategory, searchTerm) {
    $.get(url + "/dvds/" + searchCategory + "/" + searchTerm)
        .done(function (response) {
            vue.dvds = response;
        }).fail(fail);
}

function save() {

    var dvd = {
        "Title": this.current.Title,
        "ReleaseYear": this.current.ReleaseYear,
        "Director": this.current.Director,
        "Rating": this.current.Rating,
        "Notes": this.current.Notes
    };

    this.errorMessage = validate(dvd);

    if (this.errorMessage) {
        return;
    }

    if (this.current.DvdId) {
        dvd.DvdId = this.current.DvdId;
        put(dvd);
    } else {
        post(dvd);
    }
    $("#modalForm").modal("hide");
}

function validate(dvd) {
    var message = "";
    if (!dvd.Title) {
        message += "The DVD Title is required.<br />"
    }
    var regex = /^\d{4}$/;
    if (!regex.test(dvd.ReleaseYear)) {
        message += "Release Year must be a four digit number.<br />"
    }

    if (!dvd.Director) {
        message += "The Director is required.<br />"
    }
    return message;
}

var vue = new Vue({
    el: "#content",
    data: {
        searchCategory: "",
        searchTerm: "",
        searchIsInvalid: false,
        errorMessage: "",
        modalTitle: "Create DVD",
        current: {},
        dvds: []
    },
    methods: {
        confirmDelete: function (dvd) {
            this.current = dvd;
        },
        executeDelete: function () {
            remove(this.current);
        },
        create: function () {
            this.modalTitle = "Create DVD";
            this.current = {
                rating: "G"
            };
            this.errorMessage = "";
        },
        edit: function (dvd) {
            this.modalTitle = "Edit: " + dvd.Title;
            this.errorMessage = "";
            this.current = dvd;
        },
        save: save,
        search: function () {
            var valid = this.searchCategory && this.searchTerm;
            this.searchIsInvalid = !valid;
            if (valid) {
                search(this.searchCategory, this.searchTerm);
            } else {
                setTimeout(function () { vue.searchIsInvalid = false; }, 2000);
            }
        },
        clear: function () {
            this.searchCategory = "";
            this.searchTerm = "";
            getAll();
        }
    }
});

getAll();