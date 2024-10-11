<script>
export default {
    props: {
        to: {
            type: String,
            required: true,
        },
        text: {
            type: String,
            required: true,
        },
    }
};
</script>

<template>
    <LongText class="wt-long-text-link" :text="text">
        <RouterLink :to="to">{{ text }}</RouterLink>
    </LongText>
</template>

<style >
.wt-long-text-link:hover a {
    color: var(--primary-400) !important;
}
</style>

