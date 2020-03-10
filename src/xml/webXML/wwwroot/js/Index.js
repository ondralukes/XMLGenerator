var outagesFile = false;
var criticalBranchesFile = false;
var readyToSubmit = false;

function onConstraintIntervalChanged() {
    var startDate = document.getElementById('date-start').value;
    var endDate = document.getElementById('date-end').value;
    var startTime = document.getElementById('time-start').value;
    var endTime = document.getElementById('time-end').value;
    var startDateParts = startDate.split('/');
    var endDateParts = endDate.split('/');
    var res = startDateParts[2] + '-' + startDateParts[0] + '-' + startDateParts[1] + 'T' + startTime + 'Z/';
    res += endDateParts[2] + '-' + endDateParts[0] + '-' + endDateParts[1] + 'T' + endTime + 'Z'
    console.log(res);
    document.getElementById('constraintTimeInterval').value = res;
    updateSubmitButton();
}
function dragOverHandler(ev) {
    ev.preventDefault();
}
function dropHandler(ev, index) {
    ev.preventDefault();

    if (ev.dataTransfer.items) {
        for (var i = 0; i < ev.dataTransfer.items.length; i++) {
            if (ev.dataTransfer.items[i].kind === 'file') {
                var file = ev.dataTransfer.items[i].getAsFile();
                console.log('... file[' + i + '].name = ' + file.name);
                uploadFile(file, index);
            }
        }
    } else {
        for (var i = 0; i < ev.dataTransfer.files.length; i++) {
            console.log('... file[' + i + '].name = ' + ev.dataTransfer.files[i].name);
            uploadFile(file, index);
        }
    }
}
function uploadFile(file, index) {
    var url = 'upload'
    var xhr = new XMLHttpRequest()
    var formData = new FormData()
    xhr.open('POST', url, true)

    xhr.addEventListener('readystatechange', function (e) {
        if (xhr.readyState == 4 && xhr.status == 200) {
            console.log(index + "Upload OK");
            if (index == 0) {
                document.getElementById('outagesCsvFilename').value = xhr.responseText;
                document.getElementById('outagesDrop').innerHTML = file.name + ', ' + formatFileSize(file.size);
                document.getElementById('outagesDrop').classList.add('drop-ok');
                document.getElementById('outagesDrop').classList.remove('drop-fail');
                outagesFile = true;
            } else if (index == 1) {
                document.getElementById('criticalBranchesCsvFilename').value = xhr.responseText;
                document.getElementById('criticalBranchesDrop').innerHTML = file.name + ', ' + formatFileSize(file.size);
                document.getElementById('criticalBranchesDrop').classList.add('drop-ok');
                document.getElementById('criticalBranchesDrop').classList.remove('drop-fail');
                criticalBranchesFile = true;
            }

        }
        else if (xhr.readyState == 4 && xhr.status != 200) {
            console.log("Upload Error");
            if (index == 0) {
                document.getElementById('outagesCsvFilename').value = "";
                document.getElementById('outagesDrop').innerHTML = "Upload Failed!";
                document.getElementById('outagesDrop').classList.add('drop-fail');
                document.getElementById('outagesDrop').classList.remove('drop-ok');
                outagesFile = false;
            } else if (index == 1) {
                document.getElementById('criticalBranchesCsvFilename').value = "";
                document.getElementById('criticalBranchesDrop').innerHTML = "Upload Failed!";
                document.getElementById('criticalBranchesDrop').classList.add('drop-fail');
                document.getElementById('criticalBranchesDrop').classList.remove('drop-ok');
                criticalBranchesFile = false;
            }
        }
        updateSubmitButton();
    })

    formData.append('file', file);
    formData.append('index', index);


    xhr.send(formData)
}
function displayRaw() {
    document.getElementById('rawOutput').style.display = "";
    document.getElementById('viewRawBtn').style.display = "none";
}
function updateSubmitButton() {
    document.getElementById('submitButton').className = "btn btn-lg btn-block btn-outline-primary not-ready-btn";
    document.getElementById('submitButton').disabled = true;
    if (!outagesFile) {
        document.getElementById('errorMsg').innerHTML = "Outages file not uploaded!";
        return;
    }
    if (!criticalBranchesFile) {
        document.getElementById('errorMsg').innerHTML = "Critical branches file not uploaded!";
        return;
    }
    if (document.getElementById('date-start').value == '' || document.getElementById('date-end').value == '' ||
        document.getElementById('time-start').value == '' || document.getElementById('time-end').value == '') {
        document.getElementById('errorMsg').innerHTML = "ConstraintTimeInterval invalid.";
        return;
    }
    document.getElementById('errorMsg').innerHTML = "";
    document.getElementById('submitButton').className = "btn btn-primary btn-lg btn-block";
    document.getElementById('submitButton').disabled = false;
    readyToSubmit = true;
}
function submit() {
    if (readyToSubmit) {
        document.getElementById('mainForm').submit();
    }
}

function formatFileSize(bytes) {
    if (Math.abs(bytes) < 1024) {
        return bytes + ' B';
    }
    var units = ['KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB'];
    var u = -1;
    do {
        bytes /= 1024;
        u++;
    } while (Math.abs(bytes) >= 1024 && u < units.length - 1);
    return bytes.toFixed(1) + ' ' + units[u];
}
$(document).ready(function () {
    var date_input = $('input[name="date"]');
    var container = "body";
    var options = {
        format: 'mm/dd/yyyy',
        container: container,
        todayHighlight: true,
        autoclose: true,
    };
    date_input.datepicker(options);
})