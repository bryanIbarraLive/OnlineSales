$(function () {
    'use strict';
    var self = this;

    var imagePath = "/Images/";

    self.Product = function () {
        this.ID = ko.observable();
        this.Name = ko.observable();
        this.Price = ko.observable();
        this.Description = ko.observable();
        this.Type = ko.observable();
        this.Image = ko.observable();
        this.Count = ko.observable();
        this.extPrice = ko.computed(function () {
            return this.Price() * parseInt("0" + this.Count(), 10);
        }, this);
        this.imageUrl = ko.computed(function () {
            return imagePath + this.Image();
        }, this)
    };

    self.Menu = function () {
        this.type = ko.observable();
    }

    self.viewModel = function () {
        var
            products = ko.observableArray([]),

            badges = ko.observable(),

            badge = function () {
               $.ajax({
                    type: "GET",
                    url: "/Badge/Count",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data) {
                        console.log(data);
                        badges(JSON.parse(data.badge));
                    }
                });
                
            },

            productsByType = function (value) {
                var filter = value.type();
                var url = "/Products/ProductsByType/ " + filter;
                title = filter;
                window.location.href = url;
            },

            deleteFromCart = function (value) {
                var idToDelete = value.ID();
                var urlToDelete = "/ShoppingCart/DeleteFromCart/" + idToDelete;
                $.ajax({
                    type: "POST",
                    url: urlToDelete,
                    success: alert("The item was eliminated")
                });
                if (value.Count() > 1) {
                    value.Count(value.Count() - 1);
                } else {
                    products.remove(value);
                }
                badge();
            },

           types = ko.observableArray([]),

           getTypes = function () {
               self.viewModel.types.push(new self.Menu().type("Choose a type!!"));
               $.each(dataType, function (i, m) {
                   self.viewModel.types.push(new self.Menu().type(m.Type));
               });
           },

           selection = ko.observableArray(),

           selected = function (value) {
               selection.push(value);
               $("#myModal").modal();
           },

           closeDetail = function () {
               selection.removeAll();
               $("#myModal").modal('hide');
           },

           addToCart = function (value) {
               var idToAdd = value.ID();
               var urlToAdd = "/ShoppingCart/AddToCart/" + idToAdd;
               $.ajax({
                   type: "POST",
                   url: urlToAdd,
               });
               badge();
           },
           x = 0,
           loadProducts = function () {
               $.each(modelProducts, function (i, p) {
                   products.push(new self.Product()
                           .ID(p.ID)
                           .Name(p.Name)
                           .Price(p.Price)
                           .Description(p.Description)
                           .Type(p.Type)
                           .Image(p.Image)
                           .Count(modelQuantity === null ? 0 : modelQuantity[x].Counter)
                   );
                   x = x + 1;
               });


           },

          grandTotal = ko.computed(function () {
              var total = 0;
              $.each(products(), function (i, p) {
                  total += p.extPrice();
              });
              return total;
          });

        return {
            products: products,
            grandTotal: grandTotal,
            deleteFromCart: deleteFromCart,
            types: types,
            getTypes: getTypes,
            selection: selection,
            selected: selected,
            loadProducts: loadProducts,
            closeDetail: closeDetail,
            addToCart: addToCart,
            productsByType: productsByType,
            badge: badge,
            badges: badges
        };
    }();

    self.viewModel.badge();
    self.viewModel.loadProducts();
    self.viewModel.getTypes();
    ko.applyBindings(self.viewModel);
})

dataType = [
     { "Type": "Home" },
    { "Type": "Electronics" },
    { "Type": "HardwareStore" },
    { "Type": "Cars" },
    { "Type": "Drugstore" },
    { "Type": "Sports" },
    { "Type": "School" },
    { "Type": "Cosmetics" },
    { "Type": "PersonalCare" },
]

