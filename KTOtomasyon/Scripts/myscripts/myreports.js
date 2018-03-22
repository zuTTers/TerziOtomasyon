$("#btnAylikRapor").click();

function AylikRapor() {

    $.post("GetMountlyReport", 
        function (datax, status) {

            var dataViews = {

                labels: ['Oca', 'Şub', 'Mar', 'Nis', 'May', 'Haz', 'Tem', 'Agu', 'Eyl', 'Eki', 'Kas', 'Ara'],

                series: [
                    datax,

                ]

            };



            var optionsViews = {

                seriesBarDistance: 10,

                classNames: {

                    bar: 'ct-bar ct-azure'

                },

                axisX: {

                    showGrid: false

                }

            };



            var responsiveOptionsViews = [

                ['screen and (max-width: 640px)', {

                    seriesBarDistance: 5,

                    axisX: {

                        labelInterpolationFnc: function (value) {

                            return value[0];

                        }

                    }

                }]

            ];



            Chartist.Bar('#chartViews', dataViews, optionsViews, responsiveOptionsViews);

        });
}


function TarihRapor() {

    $.post("GetDateReport", {
        LastOrderDate: $("#LastOrderDate").val(),
        FirstOrderDate: $("#FirstOrderDate").val()
    },
        function (datax, status) {

            var dataViews = {

                labels: ['Toplam'],

                series: [
                    [datax]
                ]

            };

            var optionsViews = {

                seriesBarDistance: 10,

                classNames: {

                    bar: 'ct-bar ct-azure'

                },

                axisX: {

                    showGrid: false

                }

            };

            var responsiveOptionsViews = [

                ['screen and (max-width: 640px)', {

                    seriesBarDistance: 5,

                    axisX: {

                        labelInterpolationFnc: function (value) {

                            return value[0];

                        }

                    }

                }]

            ];

            Chartist.Bar('#chartViews', dataViews, optionsViews, responsiveOptionsViews);

            Bildirim(datax + "TL geliriniz var.");
        });
}
