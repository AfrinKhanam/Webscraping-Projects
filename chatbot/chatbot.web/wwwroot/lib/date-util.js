
function convertToDateObject(stringDate) {
    var input = stringDate.split("-");
    var day = input[0];
    var monthName = input[1];
    var year = input[2];

    var month = monthName;

    if (isNaN(input[1])) {
        month = "JanFebMarAprMayJunJulAugSepOctNovDec".indexOf(monthName) / 3;
    } else
        month = month - 1;
    var dateOutput = new Date(year, month, day);

    return dateOutput;
}

function formatDate(date, type) {
    var current_datetime, formatted_date;
    switch (type) {
        case "dd-mm-yyyy":
            current_datetime = convertToDateObject(date);
            formatted_date = (current_datetime.getDate()) + "-" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getFullYear();
            break;
        case "mm-dd-yyyy":
            current_datetime = convertToDateObject(date);
            formatted_date = (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate() + "-" + current_datetime.getFullYear();
            break;
        case "yyyy-mm-dd":
            current_datetime = convertToDateObject(date);
            formatted_date = current_datetime.getFullYear() + "-" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate();
            break;
        default:
    }
    return formatted_date;
}