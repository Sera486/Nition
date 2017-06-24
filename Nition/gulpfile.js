﻿/// <binding BeforeBuild='less' ProjectOpened='watch-less' />
var gulp = require("gulp"),
    del = require('del'),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    fs = require("fs"),
    less = require("gulp-less");
//препроцессинг less в css
gulp.task("less", function () {
    return gulp.src('wwwroot/less/*.less')
        .pipe(less())
        .pipe(gulp.dest('wwwroot/css'));
});
// Task to watch less changes
gulp.task('watch-less', function () {
    gulp.watch('wwwroot/less/*.less', ['less']);
});

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

gulp.task("clean:js", function () {
    del(paths.concatJsDest);
});

gulp.task("clean:css", function () {
    del(paths.concatCssDest);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);