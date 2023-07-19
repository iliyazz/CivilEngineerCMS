function statistics() {
    $("#statistics_btn").on("click",
        function(e) {
            e.preventDefault();
            e.stopPropagation();

            if ($("#statistics_box").hasClass("d-none")) {
                $.get("https://localhost:7208/api/statistics",
                    function (data) {
                        $("#total_projects").text(data.totalProjects + " projects");
                        $("#total_active_projects").text(data.totalActiveProjects + " active projects");
                        $("#total_clients").text(data.totalClients + " clients");

                        $("#statistics_box").removeClass("d-none");
                        $("#statistics_btn").text("Hide statistics");
                        $("#statistics_btn").removeClass("btn-primary");
                        $("#statistics_btn").addClass("btn-primary");
                    });
            } else {
                $("#statistics_box").addClass("d-none");
                $("#statistics_btn").text("Show statistics");
                $("#statistics_btn").removeClass("btn-primary");
                $("#statistics_btn").addClass("btn-primary");
            }
        }
    );
}

//toggleListAndCardStyle
//function toggleListAndCardStyle() {
//    $("#toggleListCardStyle_btn").on("click",
//        function(e) {
//            e.preventDefault();
//            e.stopPropagation();


//            var btn = document.getElementById("toggleListCardStyle_btn");
//            var btnText = document.getElementById("toggleListCardStyle_btn");
//            var sectionList = document.getElementById("projectListStyle");
//            var sectionCard = document.getElementById("projectCardStyle");

//            if (btnText.textContent === "To List style") {
//                btn.innerText = "To Card style";
//                sectionList.classList.remove("d-none");
//                sectionCard.classList.add("d-none");
//            } else {
//                btn.innerText = "To List style";
//                sectionList.classList.add("d-none");
//                sectionCard.classList.remove("d-none");
//            }
//        }
//    );
//}