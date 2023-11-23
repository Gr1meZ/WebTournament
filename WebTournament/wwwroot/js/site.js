const loader = {
    show() {
        $('.fullscreen-spinner').show();
    },
    hide() {
        $('.fullscreen-spinner').attr("style", 'display: none !important;');
    }
}

function dtAjaxHandler(url, data, callback) {
    $.ajax({
        url: url,
        type: 'POST',
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(dtAjaxQueryToObj(data)),
        success: function (res) {
            callback({
                recordsTotal: res.metadata.totalItemCount,
                recordsFiltered: res.metadata.totalItemCount,
                data: res.data,
                draw: data.draw
            });
        }
    })
}
function dtAjaxQueryToObj(d) {
    let orderColumn = d.order != null && d.order.length > 0 ? d.order[0].column : null;
    return {
        draw: d.draw,
        pageNumber: d.length > 0 ? (d.start / d.length) + 1 : -1,
        pageSize: d.length,
        search: d.search != null ? d.search.value : null,
        orderColumn: orderColumn != null && d.columns != null && d.columns.length > 0 ? d.columns[orderColumn].data : null,
        orderDir: orderColumn != null ? d.order[0].dir : null
    };
}

function initCustomMultiSelect(options) {
    let select2Options = {
        containerCssClass : "form-control",
        placeholder: $(this).data('placeholder'),
        multiple: false,
        tags: true,
        language: {
            noResults: function(){
                return options.helpMessage;
            }
        },
    };

    $(options.selector).select2(select2Options);
}

function initAjaxAutoCompleteSelect(options) {
    let pageSize = 30;
    console.log(options)
    let select2Options = {
        dropdownParent: $('#myModal'),
        containerCssClass : "form-control",
        placeholder: $(this).data('placeholder'),
        multiple: false,
        width: '100%',
        dropdownAutoWidth : true,
        ajax: {
            url: options.url,
            dataType: 'json',
            delay: 250,
            method: 'POST',
            data: function(params)
            {

                return {
                    search: params.term,
                    skip: (parseInt(params.page || 1) - 1) * pageSize,
                    pageSize: pageSize
                };
                cache: true
            },

            processResults: function(data, params)
            {
                params.page = params.page || 1;
                return {
                    results: data.data,
                    pagination:
                        {
                            more: (params.page * pageSize) < data.total
                        }
                };
            }
        },
        escapeMarkup: function(markup)
        {
            return markup;
        },
        minimumInputLength: 0,
        templateResult: function (item) {
            if (item.loading)
                return item.text;

            return "<div class='select2-result-repository clearfix d-flex'>" +
                "<div class='select2-result-repository__meta'>" +
                "<div class='select2-result-repository__title fs-lg fw-500'>" + item.name + "</div>"
                + "</div></div>";
        },
        templateSelection: function (item) {
            if (item.text)
                return item.text;

            return '<div data-id="'+ item.name +'"> '+item.name+'</div>';
        }
    };

    if(options.maxItems)
        select2Options.maximumSelectionLength = options.maxItems;

    $(options.selector).select2(select2Options);

}

function initAjaxAutoCompleteMultiplySelect(options) {
    let pageSize = 30;
    console.log(options)
    let select2Options = {
        containerCssClass : "form-control",
        placeholder: $(this).data('placeholder'),
        multiple: true,
        width: options.width,
        dropdownAutoWidth : true,
        ajax: {
            url: options.url,
            dataType: 'json',
            delay: 250,
            method: 'POST',
            data: function(params)
            {

                return {
                    search: params.term,
                    skip: (parseInt(params.page || 1) - 1) * pageSize,
                    pageSize: pageSize
                };
                cache: true
            },

            processResults: function(data, params)
            {
                params.page = params.page || 1;
                return {
                    results: data.data,
                    pagination:
                        {
                            more: (params.page * pageSize) < data.total
                        }
                };
            }
        },
        escapeMarkup: function(markup)
        {
            return markup;
        },
        minimumInputLength: 0,
        templateResult: function (item) {
            if (item.loading)
                return item.text;

            return "<div class='select2-result-repository clearfix '>" +
                "<div class='select2-result-repository__meta'>" +
                "<div class='select2-result-repository__title fs-lg fw-500'>" + item.name + "</div>"
                + "</div></div>";
        },
        templateSelection: function (item) {
            if (item.text)
                return item.text;

            return '<div data-id="'+ item.name +'"> '+item.name+'</div>';
        }
    };

    if(options.maxItems)
        select2Options.maximumSelectionLength = options.maxItems;

    $(options.selector).select2(select2Options);

}