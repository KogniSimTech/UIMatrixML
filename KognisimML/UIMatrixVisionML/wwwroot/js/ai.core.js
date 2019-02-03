class UIMatrixFeature {
    constructor(lbl, fn, wght = 0.5) {
        this.label = lbl;
        this.func = fn;
        this.weight = wght;
    }
}
class UIMatrix {
}
class UIMatrixElement {
    constructor(e) {
        this.jQueryElement = e;
        this.id = $(e).attr("id");
        this.name = $(e).attr("name");
        this.Matrix = new UIMatrix();
        //this.defaultPosition = [$(e).offset().left, $(e).offset().top];
    }
}
class UIMatrices {
    encode(str) {
        var num = "0x";
        var length = str.length;
        for (var i = 0; i < length; i++)
            num += str.charCodeAt(i).toString(16);
        return parseInt(num);
    }
    addFeature(feature) {
        this.features.push(feature);
    }
    addElement(e) {
        var elem = new UIMatrixElement(e);
        this.elements.push(elem);
        return elem;
    }
    addElements(e) {
        var that = this;
        var matrixElements = [];
        for (var i = 0; i < e.length; i++) {
            var elem = new UIMatrixElement(e[i]);
            matrixElements.push(elem);
            that.addElement(elem.jQueryElement);
        }
        return matrixElements;
    }
    computeMatrices() {
        var lastData = [];
        var dataSet = [];
        for (var i = 0; i < this.elements.length; i++) {
            var data = Array((this.elements.length - 1) * this.features.length).fill(0);
            for (var feature_count = 0; feature_count < this.features.length; feature_count++)
                data.push(this.features[feature_count].func($(this.elements[i].jQueryElement)) * this.features[feature_count].weight);
            var x = lastData.length - (i * this.features.length);
            if (i >= 1) {
                for (var k = x; k < lastData.length; k++)
                    data[k - this.features.length] = lastData[k];
            }
            lastData = data;
            this.elements[i].Matrix.data = data;
            dataSet.push(data);
        }
        return dataSet;
    }
    calculateMatrices(normalize = false) {
        var result = [];
        var dataSet = this.computeMatrices();
        result = dataSet[0].slice(0);
        for (var n_arr = 1; n_arr < dataSet.length; n_arr++) {
            for (var m = 0; m < dataSet[n_arr].length; m++) {
                result[m] = (result[m] * (dataSet[n_arr][m] > 0 ? dataSet[n_arr][m] : 1));
            }
        }
        return result;
    }
    disorganizeElements(messiness) {
        for (var i = 0; i < this.elements.length; i++) {
            $(this.elements[i].jQueryElement).css("marginLeft", ((parseInt($(this.elements[i].jQueryElement).css("marginLeft").replace("px", "")) + 1) * messiness) * Math.random());
            $(this.elements[i].jQueryElement).css("marginTop", ((parseInt($(this.elements[i].jQueryElement).css("marginTop").replace("px", "")) + 1) * messiness) * Math.random());
        }
    }
    constructor(initPositionFeatures = true, initSizeFeatures = true, initialFeatures = []) {
        this.features = [];
        this.elements = [];
        if (initPositionFeatures == true) {
            this.features.push(new UIMatrixFeature("pos_x", function (e) {
                return e.length == 0 ? 0 : e.offset().left;
            }));
            this.features.push(new UIMatrixFeature("pos_y", function (e) {
                return e.length == 0 ? 0 : e.offset().top;
            }));
        }
        if (initSizeFeatures == true) {
            this.features.push(new UIMatrixFeature("pos_width", function (e) {
                return e.length == 0 ? 0 : e.width();
            }));
            this.features.push(new UIMatrixFeature("pos_height", function (e) {
                return e.length == 0 ? 0 : e.height();
            }));
        }
    }
}


var matrixModel = {
    matrices: new UIMatrices(true),
    validity: null,
    init: (function () {
        matrixModel.matrices = new UIMatrices(true);
    })
};