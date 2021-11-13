Europa.Controllers.NovaRegraComissao = {};
Europa.Controllers.NovaRegraComissao.TreeViewEv = undefined;
Europa.Controllers.NovaRegraComissao.MatrizFixa = undefined;
Europa.Controllers.NovaRegraComissao.MatrizNominal = undefined;
Europa.Controllers.NovaRegraComissao.FilterRow = undefined;
Europa.Controllers.NovaRegraComissao.FilterColumn = undefined;
Europa.Controllers.NovaRegraComissao.ModalidadeFixa = 1;
Europa.Controllers.NovaRegraComissao.ModalidadeNominal = 2;
Europa.Controllers.NovaRegraComissao.IdxEv = 0;
Europa.Controllers.NovaRegraComissao.ItensValidos = false;

Europa.Controllers.NovaRegraComissao.Parametro = {
    Regional: "",
    EvsSelecionadas: [],
    EvsDiferenciadas: [],
    EvsComuns: [],
    ItensComuns: [],
    ItensDiferenciados: [],
};

$(function () {
    Europa.Controllers.NovaRegraComissao.TreeViewEv = new Europa.Components.TreeView("empresa_venda_treeview_primeira_etapa");
});

