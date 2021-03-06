import React from 'react';
import _ from "lodash";
import { Select, MenuItem, Typography, Avatar } from '@material-ui/core';
import { TASK_STATUSES } from "../constants";

const styles = {
    container: {
        // minWidth: 250,
    },
    item: {
        display: "grid",
        gridTemplateColumns: "auto 1fr",
        gridColumnGap: 8,
        alignItems: "center"
    },
    [`select${TASK_STATUSES.Open}`]: {
        background: "#bdbdbd",
        color: "white"
    },
    [`select${TASK_STATUSES["In Progress"]}`]: {
        background: "#03a9f4"
    },
    [`select${TASK_STATUSES.Review}`]: {
        background: "#9575cd"
    },
    [`select${TASK_STATUSES.Done}`]: {
        background: "#388e3c"
    },
}

export default props => {
    const { loading, status } = props;

    const onChange = e => {
        props.onChange({ status: e.target.value });
    }
    
    return (
        <Select style={styles.container} value={status} disabled={loading} onChange={onChange}>
            {_.map(TASK_STATUSES, (v, k) => (
                <MenuItem key={k} value={v}>
                    <div style={styles.item}>
                        <Avatar style={styles[`select${v}`]} />
                        <Typography variant="button">{k}</Typography>
                    </div>
                </MenuItem>))}
        </Select>     
    );                               
}