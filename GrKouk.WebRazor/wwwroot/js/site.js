// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
//I put it function closure for not poluting global namespace
var loadOptionsForSelect = () => {
    $('select[data-source]').each(function () {
        var $select = $(this);

        $select.append('<option></option>');

        $.ajax({
            url: $select.attr('data-source')
        }).then(function (options) {
            options.map(function (option) {
                var $option = $('<option>');

                $option
                    .val(option[$select.attr('data-valueKey')])
                    .text(option[$select.attr('data-displayKey')]);

                $select.append($option);
            });
        });
    });
};

var grkoukCommon = {
    editPrototypeUrl: function (curSectionCode, sectionCode, creatorId) {
        if (curSectionCode === sectionCode) {
            return "#";
        }
        if (creatorId === -1) {
            return "#";
        }
        let t = "";
        switch (sectionCode) {
            case "SCNTRANSACTORTRANS":
                t = `/transactions/TransactorTransMng/Edit?id=${creatorId}`;
                break;
            case "SCNSELLCOMBINED":
                t = `/transactions/sellmaterialdoc/Edit?id=${creatorId}`;
                break;
            case "SCNBUYMATTRANS":
                t = `/transactions/buymaterialsdoc/Edit?id=${creatorId}`;
                break;

            default:
        }
        return t;
    }

};

var myLibrary = {
    makeStartRefresher: function(refresh, refreshTime) {
        //function code
    },
    readData: function(address) {
        //function code
    }
}; 